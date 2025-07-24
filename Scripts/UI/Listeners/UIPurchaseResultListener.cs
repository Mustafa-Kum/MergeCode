using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace _Game.Scripts.UI.Listeners
{
    public class UIPurchaseResultListener : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public UnityEvent onPurchaseSuccess;
        public UnityEvent onPurchaseFailed;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.IAPEvents.PurchaseSuccess += OnPurchaseSuccess;
            EventManager.IAPEvents.PurchaseFailed += OnPurchaseFailed;
        }

        private void OnDisable()
        {
            EventManager.IAPEvents.PurchaseSuccess -= OnPurchaseSuccess;
            EventManager.IAPEvents.PurchaseFailed -= OnPurchaseFailed;
        }

        #endregion

        #region PRIVATE METHODS

        private void OnPurchaseSuccess(PurchaseEventArgs purchase)
        {
            onPurchaseSuccess?.Invoke();
        }
        
        private void OnPurchaseFailed(Product product ,PurchaseFailureReason reason)
        {
            onPurchaseFailed?.Invoke();
        }

        #endregion
    }
}