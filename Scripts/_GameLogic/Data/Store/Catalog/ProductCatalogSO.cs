using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Store.Product;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts._GameLogic.Data.Store.Catalog
{
    [CreateAssetMenu(fileName = nameof(ProductCatalogSO), menuName = "Handler Project/IAP/ProductCatalogSO", order = 0)]
    public class ProductCatalogSO : ScriptableObject
    {
        #region PUBLIC VARIABLES

        public List<PurchasableProductSO> products;

        #endregion

        #region PUBLIC METHODS

        public void Init(ConfigurationBuilder builder)
        {
            foreach (var product in products)
            {
                product.Init(builder);
            }
        }

        #endregion
    }
}