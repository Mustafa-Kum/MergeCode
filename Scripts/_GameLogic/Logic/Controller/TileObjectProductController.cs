using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Helper.Extensions.System;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class TileObjectProductController : BaseInputProvider
    {
        protected override void OnClick()
        {
        }

        protected override void OnDrag()
        {
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected override void OnRelease()
        {
            var gridTile = GetGridTileFromRayCast();
            
            TryProductObjectAction(gridTile);
            CheckTileObjectProductProduce(gridTile);
        }
        
        private void CheckTileObjectProductProduce(GridTile gridTile)
        {
            if(IsDragging()) return;
            if (gridTile == null || gridTile.IsEmpty) return;
            
            gridTile.GridObject?.OnProductCheck?.Invoke();
            
            TDebug.Log("Product check invoked");
        }
        
        private void TryProductObjectAction(GridTile gridTile)
        {
            if(IsDragging()) return;
            if (gridTile == null || gridTile.IsEmpty) return;
            if (gridTile.GridObject.TryGetComponent(out IObjectClickAction objectAction))
            {
                objectAction.Execute();
            }
        }
        
        private GridTile GetGridTileFromRayCast()
        {
            Ray ray = GetMainCamera().ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return null;

            return hit.collider.TryGetComponent(out GridTile tile) ? tile : null;
        }
    }
}