using _Game.Scripts._GameLogic.Data.Store.Product;
using _Game.Scripts.Managers.Core;
using TMPro;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class PurchasableProductView : ButtonBase
    {
        #region PUBLIC VARIABLES

        public PurchasableProductSO productSO;
        
        #endregion

        #region INSPECTOR VARIABLES

        public Image productImage;
        
        public TextMeshProUGUI productPriceText;
        
        public TextMeshProUGUI productAmountText;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            InitializeProduct();
        }
        
        #endregion

        #region PRIVATE METHODS

        private void InitializeProduct()
        {
            if (productAmountText != null && productSO.actions.Count > 0)
            {
                productAmountText.text = productSO.actions[0].amount.ToString();
            }

            productPriceText.text = productSO.currencyCode+" " + productSO.productPrice;
            
            if (productSO.productImage == null) return;
            productImage.sprite = productSO.productImage;
        }

        #endregion

        protected override void OnClicked()
        {
            EventManager.IAPEvents.PurchaseButtonClicked?.Invoke(productSO.productID);
        }
    }
}