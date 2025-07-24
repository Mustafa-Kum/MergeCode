using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    [CreateAssetMenu(fileName = nameof(GridDataContainer), menuName = "Merge Valley/Data/Grid Data Container",
        order = 0)]
    public class GridDataContainer : SerializedScriptableObject
    {
        public Dictionary<GridType, GameObject> Grids;
        public Vector2Int GridSize;
        
        [Serializable]
        public enum GridType
        {
            Type1,
            Type2
        }

        public Vector2Int GetGridSize() => GridSize;

        public GameObject GetGrid(GridType gridType) => Grids[gridType];

        public GridType GetGridType(int index) => (GridType) Enum.GetValues(typeof(GridType)).GetValue(index);
    }
}