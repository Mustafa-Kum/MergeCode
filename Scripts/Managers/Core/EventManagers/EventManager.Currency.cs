using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class Resource
        {
            #region Currency
            
            public static UnityAction<CurrencyType> CurrencyChanged;
            public static UnityAction<CurrencyType, Vector3> CurrencyCollected;

            #endregion
        }
    }
}