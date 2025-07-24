using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.RunTime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = nameof(CurrencyValuesSO), menuName = "Handler Project/Currency/CurrencyValuesSO", order = 1)]
    public class CurrencyValuesSO : PersistentSaveManager<CurrencyValuesSO>, IResettable
    {
        public Dictionary<CurrencyType, int> collectableValues = new();

        public int GetValue(CurrencyType type) => collectableValues.GetValueOrDefault(type, 0);

        [Button]
        public void AddValue(CurrencyType type, int addedValue)
        {
            if (collectableValues.ContainsKey(type))
                collectableValues[type] += addedValue;
            else
                collectableValues.Add(type, addedValue);
            
            EventManager.Resource.CurrencyChanged?.Invoke(type);
        }

        [Button]
        public void ClearCurrency(CurrencyType type)
        {
            collectableValues[type] = 0;
            
            EventManager.Resource.CurrencyChanged?.Invoke(type);
        }
        
        public int GetValue(CurrencyType type, int defaultValue) => collectableValues.GetValueOrDefault(type, defaultValue);

        public void MultiplyValue(CurrencyType type, int multiplier)
        {
            if (collectableValues.ContainsKey(type))
                collectableValues[type] *= multiplier;
            else
                collectableValues.Add(type, multiplier);
        }

        public bool SpendValue(CurrencyType type, int spentValue)
        {
            if (collectableValues.TryGetValue(type, out var currentValue))
                if (currentValue >= spentValue)
                {
                    collectableValues[type] = currentValue - spentValue;
                    return true;
                }

            return false;
        }
    }
}