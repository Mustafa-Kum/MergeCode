using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public sealed class GridTile : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private Vector2Int _gridPosition;
        [SerializeField] [ReadOnly] private GridObject _gridObject;
        [SerializeField] private GameObject _highlightObject;
        public bool IsEmpty => _gridObject == null;
        public GridObject GridObject => _gridObject;
        public Vector2Int GridPosition => _gridPosition;
        public GameObject HighlightObject => _highlightObject;
        public event Action<GridTile, GridTile> OnMergeCheck;

        public void SetTileMatrixPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public void SetTileObject(GridObject tileObject)
        {
            _gridObject = tileObject;
        }

        public void CheckMerge(GridTile sourceTile, GridTile rayCastTargetTile)
        {
            OnMergeCheck?.Invoke(sourceTile, rayCastTargetTile);
        }

        [Button]
        public void CheckMergeEditor()
        {
            CheckMerge(this,this);
        }

        private void Start() => OnMergeCheck += OnNeighborMergeCheck;

        private GridTile[] GetNeighbors()
        {
            var neighbors = new GridTile[4];
            var gridArray = RuntimeGridDataCache.GetGridArray();
            var x = _gridPosition.x;
            var y = _gridPosition.y;

            if (x > 0) neighbors[0] = gridArray[x - 1, y]; // Left
            if (x < gridArray.GetLength(0) - 1) neighbors[1] = gridArray[x + 1, y]; // Right
            if (y > 0) neighbors[2] = gridArray[x, y - 1]; // Down
            if (y < gridArray.GetLength(1) - 1) neighbors[3] = gridArray[x, y + 1]; // Up

            return neighbors;
        }

        private void OnNeighborMergeCheck(GridTile sourceTile, GridTile rayCastTargetTile)
        {
            if (_gridObject == null || sourceTile.GridObject == null) return;

            List<GridTile> neighbors = GetValidNeighbors(rayCastTargetTile);

            MergeTiles(sourceTile, neighbors);
        }

        public List<GridTile> GetValidNeighbors(GridTile tile)
        {
            List<GridTile> validNeighbors = new List<GridTile>();
            Queue<GridTile> tilesToCheck = new Queue<GridTile>();
            HashSet<GridTile> checkedTiles = new HashSet<GridTile>();

            tilesToCheck.Enqueue(tile);

            while (tilesToCheck.Count > 0)
            {
                var currentTile = tilesToCheck.Dequeue();
                if (currentTile == null || !checkedTiles.Add(currentTile)) continue;

                var currentObject = currentTile.GridObject;
                if (currentObject == null) continue;
                var currentData = currentObject.GetGridObjectData();
                var sourceData = tile.GridObject.GetGridObjectData();

                if (currentData.Type != sourceData.Type || currentData.Level != sourceData.Level) continue;
                validNeighbors.Add(currentTile);
                var neighbors = currentTile.GetNeighbors();
                foreach (var neighbor in neighbors)
                {
                    if (neighbor != null && !checkedTiles.Contains(neighbor))
                    {
                        tilesToCheck.Enqueue(neighbor);
                    }
                }
            }

            return validNeighbors;
        }

        public List<GridTile> GetValidNeighborsExceptSource(GridTile sourceTile)
        {
            List<GridTile> validNeighbors = GetValidNeighbors(sourceTile);
            validNeighbors.Remove(sourceTile);
            return validNeighbors;
        }
        
        public bool IfObjectsAreMergeable(GridObject.GridObjectData sourceData)
        {
            return sourceData.Level is < 4 and > 0;
        }
        
        public bool IsTileSameTypeWithOtherTile(GridTile otherTile)
        {
            if (GridObject == null || otherTile.GridObject == null) return false;
            return GridObject.GetGridObjectData().Type == otherTile.GridObject.GetGridObjectData().Type;
        }

        public void HighlightTile(bool condition)
        {
            _highlightObject.SetActive(condition);
        }

        private void MergeTiles(GridTile sourceTile, List<GridTile> tiles)
        {
            List<GridTile> validNeighbors = GetValidNeighbors(sourceTile);
            float delay = 0.25f;
            int mergeableCount = tiles.Count;
            if (validNeighbors.Count > 1)
            {
                mergeableCount = tiles.Count;
            }
            else
            {
                mergeableCount++;

            }
            
            DisableColliders(tiles, sourceTile);
            
            Vector3 targetPosition =
                CalculateTargetPosition(tiles[0].transform.position, sourceTile.transform.position.y);
            GridObject.GridObjectData data = sourceTile.GridObject.GetGridObjectData();
            SendObjectMergedInterfaceSignal(sourceTile.GridObject);
            Vector2Int tempPosition = tiles[0]._gridPosition;
            tiles.ForEach(x => x._gridObject.transform.DOMove(targetPosition, delay).OnComplete(() => ResetTile(x, sourceTile)));

            DOVirtual.DelayedCall(delay + 0.1f, () =>
            {
                EventManager.GridEvents.OnGridObjectMerged?.Invoke(data, tempPosition, mergeableCount);
                EventManager.LevelProgressEvents.OnLevelProgressUpdated?.Invoke(10);
                EventManager.GridEvents.OnGridObjectMerge?.Invoke(tiles[0]);;
            });
        }

        private Vector3 CalculateTargetPosition(Vector3 basePosition, float yPosition) => new(basePosition.x - 0.5f, yPosition + 1.1f, basePosition.z - 0.75f);

        private void DisableColliders(List<GridTile> tiles, GridTile sourceTile)
        {
            tiles.ForEach(t => t.GetComponent<Collider>().enabled = false);
            sourceTile.GetComponent<Collider>().enabled = false;
        }
        
        private void SendObjectMergedInterfaceSignal(GridObject obj)
        {
            obj.GetGridObjectClickableActions().ForEach(action => action.OnObjectMerge(obj));
        }

        private void ResetTile(GridTile tile, GridTile sourceTile)
        {
            tile.GetComponent<Collider>().enabled = true;
            sourceTile.GetComponent<Collider>().enabled = true;
            Destroy(sourceTile.GridObject.gameObject);
            Destroy(tile.GridObject.gameObject);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}