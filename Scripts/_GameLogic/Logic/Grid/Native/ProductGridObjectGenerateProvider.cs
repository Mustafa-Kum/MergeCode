using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Grid.Currencies;
using _Game.Scripts.Helper.Extensions.System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public class ProductGridObjectGenerateProvider
    {
        private readonly int _numberOfProductsToSpawn;
        private readonly GridObjectProductDataContainer _productDataContainer;
        private readonly CurrencyObjectDataContainer _currencyObjectDataContainer;
        private const float TweenDuration = 0.15f;

        public ProductGridObjectGenerateProvider(GridObjectProductDataContainer productDataContainer, CurrencyObjectDataContainer currencyObjectDataContainer, int numberOfProductsToSpawn)
        {
            _productDataContainer = productDataContainer;
            _currencyObjectDataContainer = currencyObjectDataContainer;
            _numberOfProductsToSpawn = numberOfProductsToSpawn;
        }

        public void GenerateProductObject(GridTile source, GridObjectProductDataContainer.ProductType productType)
        {
            Sequence animationSequence = DOTween.Sequence();
            List<GridTile> emptyTiles = RuntimeGridDataCache.FindEmptyNeighborTilesFromSource(source);
    
            if (emptyTiles.Count < _numberOfProductsToSpawn)
            {
                TDebug.LogWarning("Not enough empty tiles to spawn product.");
                return;
            }
    
            for (int i = 0; i < _numberOfProductsToSpawn; i++)
            {
                var tile = emptyTiles[i];
                Product prefab = _productDataContainer.GetProductPrefab(productType); 
                var position = new Vector3(tile.transform.position.x, 0.8f, tile.transform.position.z); 
                
                Product product = Object.Instantiate(prefab, source.transform.position, prefab.transform.rotation);
                GridObjectProductDataContainer.ProductType type = product.GetGridObjectProductData().ProductType;
                
                Tween tween = product.transform.DOJump(position, 0.5f, 1, TweenDuration).SetLink(product.gameObject);
                animationSequence.Join(tween);
                
                tile.SetTileObject(product);
                product.SetCurrentTile(tile);
                
                RuntimeGridDataCache.AddProduct(type, product);
                
                UnityEngine.Debug.Log($"Product {productType} spawned at {tile.GridPosition}", tile);
            }
        }
        
        public void GenerateCurrencyObject(GridTile source)
        {
            Sequence animationSequence = DOTween.Sequence();

            GridObject prefab = _currencyObjectDataContainer.GetCurrencyPrefab(CurrencyType.Coin);
            var position = new Vector3(source.transform.position.x, 0.8f, source.transform.position.z);
            
            GridObject coin = Object.Instantiate(prefab, source.transform.position, prefab.transform.rotation);
            Tween tween = coin.transform.DOJump(position, 0.5f, 1, TweenDuration).SetLink(coin.gameObject);
            animationSequence.Append(tween);
            
            source.SetTileObject(coin);
            coin.SetCurrentTile(source);
            
            //Debug.Log($"Reward spawned at {source.GridPosition}", source);
        }
    }
}