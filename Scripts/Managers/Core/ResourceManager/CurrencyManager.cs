using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts.Managers.Core.StoreManager;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core.ResourceManager
{
    public class CurrencyManager : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private CurrencyValuesSO _currencyValuesSo;
        [SerializeField] private ProductCatalogSO _productCatalog;

        #endregion

        #region PRIVATE VARIABLES

        private IAPCurrencyStore _iapCurrencyStore;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _iapCurrencyStore = new IAPCurrencyStore(_currencyValuesSo, _productCatalog);
            _iapCurrencyStore.InitializeProducts();
        }

        private void OnEnable()
        {
            EventManager.IAPEvents.PurchaseSuccess += HandlePurchaseSuccessEvent;
            EventManager.ProductEvents.ProductCurrencyRewardCollect += AddCurrency;
        }

        private void OnDisable()
        {
            EventManager.IAPEvents.PurchaseSuccess -= HandlePurchaseSuccessEvent;
            EventManager.ProductEvents.ProductCurrencyRewardCollect -= AddCurrency;
        }

        #endregion

        #region PRIVATE METHODS

        private void HandlePurchaseSuccessEvent(PurchaseEventArgs arg0)
        {
            _iapCurrencyStore.ProcessPurchase(arg0.purchasedProduct.definition.id);
        }
        
        private void AddCurrency(CurrencyType type)
        {
            _currencyValuesSo.AddValue(type, 1);
            
            EventManager.Resource.CurrencyChanged?.Invoke(type);
        }

        #endregion
    }
}

public enum CurrencyType
{
    Coin,
    Gem,
    Energy
}