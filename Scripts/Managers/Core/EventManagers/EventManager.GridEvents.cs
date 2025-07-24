using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Grid;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using Product = _Game.Scripts._GameLogic.Logic.Grid.Currencies.Product;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class GridEvents
        {
            public static UnityAction OnGridTileClicked;
            public static UnityAction<bool> OnGridTileReleased;
            public static UnityAction<GridObject.GridObjectData, Vector2Int, int> OnGridObjectMerged;
            public static UnityAction<GridTile,GridObjectProductDataContainer.ProductType> OnProductProduced;
            public static UnityAction<GridTile> OnProductCurrencyReward;
            public static UnityAction<GridTile> OnGridObjectMerge;
        }

        public static class ProductEvents
        {
            public static UnityAction<CurrencyType> ProductCurrencyRewardCollect;
            public static UnityAction<GridObjectProductDataContainer.ProductType, Vector3> OnProductCollect;
            public static UnityAction<GridObjectProductDataContainer.ProductType,List<Product>> OnProductGenerateParticleImage;
        }
    }
}