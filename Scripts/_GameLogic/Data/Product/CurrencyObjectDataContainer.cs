using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Product
{
    [CreateAssetMenu(fileName = nameof(CurrencyObjectDataContainer), menuName = "Merge Valley/Data/Currency Object Data Container", order = 0)]
    public class CurrencyObjectDataContainer : SerializedScriptableObject
    {
        public Dictionary<CurrencyType, GridObject> CurrencyPrefabs;
        
        public GridObject GetCurrencyPrefab(CurrencyType type)
        {
            return CurrencyPrefabs[type];
        }
    }
}