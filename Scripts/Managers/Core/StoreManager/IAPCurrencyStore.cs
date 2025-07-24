using System;
using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts._GameLogic.Data.Store.Product;
using _Game.Scripts.ScriptableObjects.Saveable;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public sealed class IAPCurrencyStore : BasePurchaseProcessor
    {
        #region PRIVATE VARIABLES

        private readonly CurrencyValuesSO _currencyValuesSo;
        private readonly ProductCatalogSO _productCatalogSo;

        #endregion

        #region CONSTRUCTOR

        public IAPCurrencyStore(
            CurrencyValuesSO currencyValuesSo, 
            ProductCatalogSO productCatalogSo)
        {
            _currencyValuesSo = currencyValuesSo;
            _productCatalogSo = productCatalogSo;
        }

        #endregion

        #region PUBLIC METHODS

        public void InitializeProducts()
        {
            InitializeProductActions();
        }

        #endregion

        #region PRIVATE METHODS

        private void InitializeProductActions()
        {
            foreach (var product in _productCatalogSo.products)
            {
                productActions.Add(product.productID, () => ExecuteProductActions(product));
            }
        }


        private void ExecuteProductActions(PurchasableProductSO product)
        {
            foreach (var action in product.actions)
            {
                switch (action.actionType)
                {
                    case ProductActionType.AddCoinCurrency:
                        _currencyValuesSo.AddValue(CurrencyType.Coin, action.amount);
                        break;
                    case ProductActionType.AddGemCurrency:
                        _currencyValuesSo.AddValue(CurrencyType.Gem, action.amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        #endregion
    }
}