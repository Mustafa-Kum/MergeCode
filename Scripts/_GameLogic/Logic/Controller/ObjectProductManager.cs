using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Grid.Currencies;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class ObjectProductManager : MonoBehaviour
    {
        [SerializeField] private GridObjectProductDataContainer _gridObjectProductDataContainer;

        private void Awake() => _gridObjectProductDataContainer.ClearProductData();

        private void OnEnable() => EventManager.ProductEvents.OnProductCollect += OnProductCollect;

        private void OnDisable() => EventManager.ProductEvents.OnProductCollect -= OnProductCollect;

        private void OnProductCollect(GridObjectProductDataContainer.ProductType productType, Vector3 position)
        {
            List<Product> products = RuntimeGridDataCache.GetAllProductsInType(productType);
            int productAmount = products.Count;
            _gridObjectProductDataContainer.AddProduct(productType, productAmount);
            
            EventManager.ProductEvents.OnProductGenerateParticleImage?.Invoke(productType ,products);
            
            DestroyCollectedProducts(products, productType);
        }
        
        private void DestroyCollectedProducts(List<Product> products, GridObjectProductDataContainer.ProductType type)
        {
            RuntimeGridDataCache.RemoveProduct(type);
            
            DOVirtual.DelayedCall(0.1f, () =>
            {
                foreach (Product product in products)
                {
                    product.SelfDestruct();
                }
            });
        }
    }
}