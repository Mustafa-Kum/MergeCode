using System;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public class IAPBundlePurchaseListener : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public IAPBundleValuesSO iapBundleValuesSo;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.IAPEvents.PurchaseSuccess += HandlePurchaseSuccessEvent;
        }

        private void OnDisable()
        {
            EventManager.IAPEvents.PurchaseSuccess -= HandlePurchaseSuccessEvent;
        }

        #endregion

        #region PRIVATE METHODS

        private void HandlePurchaseSuccessEvent(PurchaseEventArgs purchase)
        {
            if (iapBundleValuesSo.IsBundlePurchased(purchase.purchasedProduct.definition.id))
            {
                Debug.LogError($"Bundle already purchased: {purchase.purchasedProduct.definition.id}");
            }
            else
            {
                EventManager.IAPEvents.BundlePurchased?.Invoke(purchase);
                if (purchase.purchasedProduct.definition.type == ProductType.Consumable)
                {
                    return;
                }
                iapBundleValuesSo.WriteBundle(purchase.purchasedProduct.definition.id);
            }
        }

        #endregion
    }
}