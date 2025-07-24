using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public abstract class BasePurchaseProcessor
    {
        #region PRIVATE VARIABLES

        protected readonly Dictionary<string, Action> productActions = new Dictionary<string, Action>();

        #endregion

        #region PUBLIC METHODS

        public void ProcessPurchase(string productId)
        {
            if (productActions.TryGetValue(productId, out Action action))
            {
                action.Invoke();
            }
            else
            {
                Debug.LogError($"Product ID not found: {productId}");
            }
        }

        #endregion
    }
}