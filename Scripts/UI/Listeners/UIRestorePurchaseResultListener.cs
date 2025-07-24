using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.UI.Listeners
{
    public class UIRestorePurchaseResultListener : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public UnityEvent onPurchaseRestoreSuccess;
        public UnityEvent onPurchaseRestoreFailed;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.UIEvents.PurchaseRestoredResult += OnPurchaseRestoredResult;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.PurchaseRestoredResult -= OnPurchaseRestoredResult;
        }

        #endregion

        #region PRIVATE METHODS

        private void OnPurchaseRestoredResult(bool result)
        {
            if (result)
            {
                onPurchaseRestoreSuccess?.Invoke();
            }
            else
            {
                onPurchaseRestoreFailed?.Invoke();
            }
        }
        
        #endregion
    }
}