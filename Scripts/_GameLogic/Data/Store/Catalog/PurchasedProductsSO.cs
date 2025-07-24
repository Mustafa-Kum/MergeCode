using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.Store.Catalog
{
    [CreateAssetMenu(fileName = "PurchasedProductsSO", menuName = "Handler Project/IAP/PurchasedProductsSO", order = 0)]
    public class PurchasedProductsSO : PersistentSaveManager<PurchasedProductsSO>, IResettable
    {
        #region PUBLIC VARIABLES

        public List<string> purchasedProductIds = new List<string>();

        #endregion

        #region PUBLIC METHODS

        public void WriteProduct(string productId)
        {
            purchasedProductIds.Add(productId);
        }
        
        public bool IsProductPurchased(string productId)
        {
            return purchasedProductIds.Contains(productId);
        }
        
        public void ClearPurchasedProducts()
        {
            purchasedProductIds.Clear();
        }

        #endregion
    }
}