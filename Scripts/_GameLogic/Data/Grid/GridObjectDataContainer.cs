using System.Collections.Generic;
using System.Linq;
using _Game.Scripts._GameLogic.Logic.Grid;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Grid
{
    [CreateAssetMenu(fileName = nameof(GridObjectDataContainer), menuName = "Merge Valley/Data/Grid Object Data Container", order = 0)]
    public class GridObjectDataContainer : SerializedScriptableObject
    {
        public List<GridObjectTypeData> GridObjects;

        [System.Serializable]
        public class GridObjectLevelData
        {
            public int Level;
            public GridObject Prefab; 
        }

        [System.Serializable]
        public class GridObjectTypeData
        {
            public GridObjectType Type;
            public List<GridObjectLevelData> Levels;
        }

        private Dictionary<GridObjectType, int> _typeCounts;

        private void OnEnable()
        {
            InitializeTypeCounts();
        }

        private void InitializeTypeCounts()
        {
            _typeCounts = new Dictionary<GridObjectType, int>();
            foreach (var typeData in GridObjects)
            {
                _typeCounts[typeData.Type] = 0;
            }
        }
        
        [Pure] [LinqTunnel]
        public GridObject GetBalancedRandomTypeFirstLevelGridObject()
        {
            var leastUsedTypes = _typeCounts.Where(x => x.Value == _typeCounts.Values.Min()).Select(x => x.Key).ToList();
            var selectedType = leastUsedTypes[Random.Range(0, leastUsedTypes.Count)];
            _typeCounts[selectedType]++;
            var gridObjectTypeData = GridObjects.Find(x => x.Type == selectedType);
            return gridObjectTypeData.Levels[0].Prefab;
        }
        
        [Pure]
        public GridObject GetNextLevelGridObject(GridObject.GridObjectData gridObjectData)
        {
            var gridObjectTypeData = GridObjects.Find(x => x.Type == gridObjectData.Type);
            var nextLevel = gridObjectData.Level + 1;
            var nextLevelData = gridObjectTypeData.Levels.Find(x => x.Level == nextLevel);
            return nextLevelData?.Prefab;
        }
        
        [Pure]
        public GridObject GetCurrentLevelGridObject(GridObject.GridObjectData data)
        {
            var gridObjectTypeData = GridObjects.Find(x => x.Type == data.Type);
            var currentLevelData = gridObjectTypeData.Levels.Find(x => x.Level == data.Level);
            if (currentLevelData == null)
            {
                Debug.LogError($"No level data found for {data.Type} at level {data.Level}");
            }
            
            return currentLevelData?.Prefab;
        }
        
        public enum GridObjectType
        {
            Cow,
            Chicken,
            Crop,
            Wood,
            Brick,
            None
        }
    }
}
