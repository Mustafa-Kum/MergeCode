using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class TileObjectDragController : BaseInputProvider
    {
        private GridObject _gridObject;
        private bool HasObject => _gridObject;

        private GridTile _currentTile;
        private readonly List<GridTile> _validNeighborsOnClick = new();

        protected override void OnClick()
        {
            GridTile gridTile = GetGridTileFromRayCast();

            if (gridTile == null || gridTile.IsEmpty) return;
            
            _validNeighborsOnClick.AddRange(gridTile.GetValidNeighborsExceptSource(gridTile));

            _gridObject = gridTile.GridObject;
            _currentTile = gridTile;
            
            SendObjectClickedInterfaceSignal(_gridObject);
            EventManager.GridEvents.OnGridTileClicked?.Invoke();
        }

        protected override void OnDrag()
        {
            if (!HasObject) return;
            GridTile gridTile = GetGridTileFromRayCast();
            if (gridTile == null)
            {
                MoveObjectToMousePosition(_gridObject);
                RuntimeGridDataCache.DisableAllTilesHighlight(RuntimeGridDataCache.GetGridArray());
            }
            else
            {
                _gridObject.EnableGridObjectOutline(true);
                MoveObjectToGridTilePosition(_gridObject, gridTile);
                HandleSameTypeHighlight(gridTile);
                HandleDragHighlight(gridTile);
            }
        }

        protected override void OnRelease()
        {
            if (!HasObject) return;
            
            GridTile gridTile = GetGridTileFromRayCast();

            HandleGridTileOnRelease(gridTile);
            
            RuntimeGridDataCache.DisableAllTilesHighlight(RuntimeGridDataCache.GetGridArray());
            
            _gridObject.EnableGridObjectOutline(false);
            _gridObject = null;
            _currentTile = null;
            
             _validNeighborsOnClick.Clear();

            EventManager.GridEvents.OnGridTileReleased?.Invoke(HasObject);
        }

        private GridTile GetGridTileFromRayCast()
        {
            Ray ray = GetMainCamera().ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return null;

            return hit.collider.TryGetComponent(out GridTile tile) ? tile : null;
        }

        private void HandleGridTileOnRelease(GridTile releasedTile)
        {
            if(releasedTile == _currentTile) return;
            if (releasedTile == null)
            {
                ResetObjectPosition();
                return;
            }

            if (!releasedTile.IsEmpty && releasedTile.GridObject != null)
            {
                ProcessNonEmptyTile(releasedTile);
            }
            else
            {
                ProcessEmptyTile(releasedTile);
            }
        }

        private void ProcessNonEmptyTile(GridTile releasedTile)
        {
            GridObject.GridObjectData releasedObjectData = _gridObject.GetGridObjectData();
            GridObject.GridObjectData targetObjectData = releasedTile.GridObject.GetGridObjectData();

            if (releasedObjectData.Type == targetObjectData.Type &&
                releasedObjectData.Level == targetObjectData.Level)
            {
                HandleSameTypeAndLevelObjects(releasedTile);
            }
            else
            {
                PerformSwapOperation(releasedTile);
            }
        }

        private void HandleSameTypeAndLevelObjects(GridTile releasedTile)
        {
            List<GridTile> validNeighbors = releasedTile.GetValidNeighbors(releasedTile);
            if (_validNeighborsOnClick.Count == 0)
                validNeighbors.Add(_currentTile);

            if (validNeighbors.Count >= 3 && releasedTile.IfObjectsAreMergeable(_gridObject.GetGridObjectData()))
            {
                releasedTile.CheckMerge(_currentTile, releasedTile);
            }
            else
                ResetObjectPosition();
        }
        
        private void HandleSameTypeHighlight(GridTile rayCastTile)
        {
            RuntimeGridDataCache.DisableAllTilesHighlight(RuntimeGridDataCache.GetGridArray());
            
            List<GridTile> validNeighbors = rayCastTile.GetValidNeighbors(rayCastTile);
            if (_validNeighborsOnClick.Count == 0)
                validNeighbors.Add(_currentTile);
            
            if (_currentTile.IsTileSameTypeWithOtherTile(rayCastTile))
            {
                if (validNeighbors.Count >= 3 && rayCastTile.IfObjectsAreMergeable(_gridObject.GetGridObjectData()))
                {
                    validNeighbors.ForEach(tile => tile.HighlightTile(true));
                    _currentTile.HighlightTile(false);
                }
            }
            else
            {
                RuntimeGridDataCache.DisableAllTilesHighlight(RuntimeGridDataCache.GetGridArray());
            }
            
            validNeighbors.Clear();
        }
        
        private void HandleDragHighlight(GridTile rayCastTile)
        {
            rayCastTile.HighlightTile(true);
        }

        private void ProcessEmptyTile(GridTile releasedTile)
        {
            MoveObjectToGridTilePosition(_gridObject, releasedTile);
            _gridObject.SetCurrentTile(releasedTile);
            releasedTile.SetTileObject(_gridObject);
            _currentTile.SetTileObject(null);
        }

        private void ResetObjectPosition()
        {
            MoveObjectToGridTilePosition(_gridObject, _currentTile);
        }

        private void PerformSwapOperation(GridTile gridTile)
        {
            GridObject tempObject = gridTile.GridObject;
            gridTile.SetTileObject(_gridObject);
            _currentTile.SetTileObject(tempObject);
            
            _gridObject.SetCurrentTile(gridTile);
            tempObject.SetCurrentTile(_currentTile);
            
            _gridObject.EnableGridObjectOutline(false);
            tempObject.EnableGridObjectOutline(false);
            
            MoveObjectToGridTilePosition(tempObject, _currentTile);
            MoveObjectToGridTilePosition(_gridObject, gridTile);
        }

        private void MoveObjectToGridTilePosition(GridObject obj, GridTile tile)
        {
            GridObjectDataContainer.GridObjectType objType = obj.GetGridObjectData().Type;
            
            var offsetDictionary = new Dictionary<GridObjectDataContainer.GridObjectType, Vector3>
            {
                { GridObjectDataContainer.GridObjectType.Brick, new Vector3(-0.5f, 0, -0.75f) },
                { GridObjectDataContainer.GridObjectType.Wood, new Vector3(-1f, 0, -1.25f) },
            };

            Vector3 offset = offsetDictionary.TryGetValue(objType, out var value) ? value : Vector3.zero;
            Vector3 targetPosition = new Vector3(tile.transform.position.x, obj.transform.position.y,
                tile.transform.position.z) + offset;

            obj.transform.DOMove(targetPosition, 0.1f).SetLink(obj.gameObject);
        }
        
        private void SendObjectClickedInterfaceSignal(GridObject obj)
        {
            obj.GetGridObjectClickableActions().ForEach(action => action.OnObjectClick(obj));
        }
        
        private void MoveObjectToMousePosition(GridObject obj)
        {
            Ray ray = GetMainCamera().ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;
            Vector3 targetPosition = new Vector3(hit.point.x - 3, obj.transform.position.y, hit.point.z - 3f);
            obj.transform.DOMove(targetPosition, 0.1f).SetLink(obj.gameObject);
            obj.EnableGridObjectOutline(false);
        }
    }
}
