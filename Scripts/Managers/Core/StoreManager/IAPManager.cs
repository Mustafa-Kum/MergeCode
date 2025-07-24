using _Game.Scripts._GameLogic.Data.Store.Catalog;
using _Game.Scripts.Helper.Extensions.System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public class IAPManager : MonoBehaviour, IDetailedStoreListener
    {
        #region PRIVATE VARIABLES

        private IStoreController _mStoreController;
        private IExtensionProvider _mStoreExtensionProvider;
        private ConfigurationBuilder _builder;
        private const string RevenueCatApiKey = "appl_BDPPiOFLWTgxgraFmhYInEgycMQ";

        #endregion

        #region PUBLIC VARIABLES

        public ProductCatalogSO productCatalog;
        public PurchasedProductsSO purchasedProducts;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            InitializePurchasing();  
        }

        private void OnEnable()
        {
            EventManager.IAPEvents.PurchaseButtonClicked += PurchaseProduct;   
            EventManager.IAPEvents.PurchaseRestoredButtonClicked += RestorePurchases;
        }

        private void OnDisable()
        {
            EventManager.IAPEvents.PurchaseButtonClicked -= PurchaseProduct;
            EventManager.IAPEvents.PurchaseRestoredButtonClicked -= RestorePurchases;
        } 

        #endregion

        #region PRIVATE METHODS
        
        private void InitializePurchasing()
        {
            _builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            
            productCatalog.Init(_builder);
            
            UnityPurchasing.Initialize(this, _builder);
        }
        
        private void PurchaseProduct(string productId)
        {
            _mStoreController.InitiatePurchase(productId);   
        }
        
        private void RestorePurchases()
        {
            if (_mStoreController == null || _mStoreExtensionProvider == null)
            {
                Debug.LogError("RestorePurchases FAIL. Not initialized.");
            }
            else
            {
                Debug.Log("RestorePurchases Initialized ...");
                
                if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    Debug.Log("RestorePurchases started ...");

                    var apple = _mStoreExtensionProvider.GetExtension<IAppleExtensions>();
                    apple.RestoreTransactions((result, message) =>
                    {
                        if (result)
                        {
                            Debug.Log("Restoration process succeeded. Message: " + message);
                        }
                        else
                        {
                            Debug.Log("Restoration process failed. Message: " + message);
                        }
                    
                        EventManager.UIEvents.PurchaseRestoredResult?.Invoke(result);
                    });
                }
                else
                {
                    Debug.Log("RestorePurchases not supported on this platform. Current platform: " + Application.platform);
                }
            }
        }

        #endregion

        #region PUBLIC METHODS
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            TDebug.Log("In-App Purchasing successfully initialized");
            _mStoreController = controller;
            _mStoreExtensionProvider = extensions;
            
            UpdateProductPrices();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            OnPurchaseFailed(product, failureDescription.reason);
            //Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
              //        $" Purchase failure reason: {failureDescription.reason}," +
                //      $" Purchase failure details: {failureDescription.message}");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
            Debug.LogError("In-App Purchasing failed to initialize.");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseInfo)
        {
            Product product = purchaseInfo.purchasedProduct;

            purchasedProducts.WriteProduct(product.definition.id);
            EventManager.IAPEvents.PurchaseSuccess?.Invoke(purchaseInfo);

            //Debug.Log($"Purchase Complete - Product: {product.definition.id}");
            //Debug.Log($"Receipt: {purchaseInfo.purchasedProduct.receipt}");
            
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            //Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
              //        $" Purchase failure reason: {failureReason}," +
                //      $" Purchase failure details: {failureReason}");
            
            EventManager.IAPEvents.PurchaseFailed?.Invoke(product, failureReason);
        }
        
        private void UpdateProductPrices()
        {
            foreach (var purchasableProduct in productCatalog.products)
            {
                var product = _mStoreController.products.WithID(purchasableProduct.productID);
                if (product != null)
                {
                    purchasableProduct.productPrice = (double)product.metadata.localizedPrice;
                    purchasableProduct.currencyCode = product.metadata.isoCurrencyCode;

                    //Debug.Log($"Updated price for {purchasableProduct.productName}: {purchasableProduct.productPrice} {purchasableProduct.currencyCode}");
                }
                else
                {
                    Debug.LogWarning($"Product {purchasableProduct.productID} not found in store.");
                }
            }
        }

        
        #endregion
    }
}
