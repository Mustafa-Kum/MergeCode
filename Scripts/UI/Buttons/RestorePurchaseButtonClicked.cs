using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.UI.Buttons
{
    public class RestorePurchaseButtonClicked : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.IAPEvents.PurchaseRestoredButtonClicked?.Invoke();
        }
    }
}