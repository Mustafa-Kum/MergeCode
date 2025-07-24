using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts._GameLogic.Data.Store.Product;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts._GameLogic.Logic.Booster.UI
{
    public class PurchasedProductButtonHandler : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public PurchasableProductSO productSO;
        
        public PurchasedProductsSO purchasedProductsSo;
        
        public Transform productParent;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.IAPEvents.PurchaseSuccess += NonConsumableItemPurchaseCheck;
            
            NonConsumableItemPurchaseCheck();
        }
        
        private void OnDisable()
        {
            EventManager.IAPEvents.PurchaseSuccess -= NonConsumableItemPurchaseCheck;
        }

        #endregion
        
        #region PRIVATE METHODS

        private void NonConsumableItemPurchaseCheck(PurchaseEventArgs args = null)
        {
            if (productSO.productType != ProductType.NonConsumable) return;
            if (purchasedProductsSo.IsProductPurchased(productSO.productID))
            {
                productParent.gameObject.SetActive(false);   
            }
        }

        #endregion
    }
}