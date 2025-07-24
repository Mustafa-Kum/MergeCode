using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace _Game.Scripts._GameLogic.Data.Product
{
    [CreateAssetMenu(fileName = nameof(GridObjectProductDataContainer), menuName = "Merge Valley/Data/Grid Object Product Data Container", order = 0)]
    public class GridObjectProductDataContainer : SerializedScriptableObject
    {
        public Dictionary<ProductType, int> ObjectProductData;
        public Dictionary<ProductType, Logic.Grid.Currencies.Product> ProductPrefabs;

        private void OnEnable()
        {
            InitializeTypeCounts();
        }

        private void InitializeTypeCounts()
        {
            ObjectProductData = new Dictionary<ProductType, int>();
            foreach (ProductType type in System.Enum.GetValues(typeof(ProductType)))
            {
                ObjectProductData[type] = 0;
            }
        }
        
        public Logic.Grid.Currencies.Product GetProductPrefab(ProductType type)
        {
            return ProductPrefabs[type];
        }
        
        public void AddProduct(ProductType type, int amount)
        {
            ObjectProductData[type] += amount;
        }
        
        public void RemoveProduct(ProductType type, int amount)
        {
            ObjectProductData[type] -= amount;
        }
        
        public int GetProductAmount(ProductType type)
        {
            return ObjectProductData[type];
        }
        
        public void ClearProductData()
        {
            foreach (ProductType type in System.Enum.GetValues(typeof(ProductType)))
            {
                ObjectProductData[type] = 0;
            }
        }

        public enum ProductType
        {
            Egg,
            Milk,
            Wheat,
            Brick,
            Wood
        }
    }
}