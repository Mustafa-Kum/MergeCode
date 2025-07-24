using System;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridInitializer : SerializedMonoBehaviour
    {
        [SerializeField] private GridDataContainer _gridDataContainer;

        [TableMatrix(HorizontalTitle = "Grid Elements", SquareCells = true, ResizableColumns = false)]
        [SerializeField, ReadOnly]
        private GridTile[,] _gridArray;

        private void Awake()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            var gridSize = _gridDataContainer.GetGridSize();
            _gridArray = new GridTile[gridSize.x, gridSize.y];
            ProduceGrid();
        }

        private void ProduceGrid()
        {
            var gridSize = _gridDataContainer.GetGridSize();
            var gridTypes = Enum.GetValues(typeof(GridDataContainer.GridType));
            for (var y = 0; y < gridSize.y; y++)
            {
                for (var x = 0; x < gridSize.x; x++)
                {
                    var gridTypeIndex = (x + y) % gridTypes.Length;
                    var gridType = _gridDataContainer.GetGridType(gridTypeIndex);
                    GameObject gridElement = ProduceSingleGridElement(gridType);
                    
                    if (gridElement == null) continue;
                    GridTile tile = gridElement.GetComponent<GridTile>();
                    _gridArray[x, y] = tile;
                    InitializeGridElement(gridElement, tile, x, y);
                }
            }
            
            RuntimeGridDataCache.SetGridArray(_gridArray);
        }

        private GameObject ProduceSingleGridElement(GridDataContainer.GridType gridType)
        {
            var gridPrefab = _gridDataContainer.GetGrid(gridType);
#if UNITY_EDITOR
            var gridElement = PrefabUtility.InstantiatePrefab(gridPrefab) as GameObject;
#endif
#if !UNITY_EDITOR
            var gridElement = Instantiate(gridPrefab);
#endif
            return gridElement;
        }
        
        private void InitializeGridElement(GameObject gridElement, GridTile tile, int x, int y)
        {
            gridElement.transform.position = 
                new Vector3(x * gridElement.transform.localScale.x, 
                    0, 
                    y * gridElement.transform.localScale.z);
            gridElement.transform.SetParent(transform);

            if (tile != null)
            {
                tile.SetTileMatrixPosition(new Vector2Int(x, y));
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_gridArray == null) return;
            var gridSize = _gridDataContainer.GetGridSize();
            for (var y = 0; y < gridSize.y; y++)
            {
                for (var x = 0; x < gridSize.x; x++)
                {
                    var tile = _gridArray[x, y];
                    if (tile == null) continue;
                    var position = tile.transform.position;
                    Gizmos.DrawWireCube(position, tile.transform.localScale);
                    Gizmos.color = Color.red;
                }
            }
        }
    }
}