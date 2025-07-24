using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class IAPEvents
        {
            public static UnityAction<PurchaseEventArgs> PurchaseSuccess;
            
            public static UnityAction<Product, PurchaseFailureReason> PurchaseFailed;
            
            public static UnityAction<string> PurchaseButtonClicked;
            
            public static UnityAction PurchaseRestoredButtonClicked;
            
            public static UnityAction<PurchaseEventArgs> BundlePurchased;
        }
    }
}