using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using Sirenix.OdinInspector;
using TMPro;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class ProductInventoryUIManager : SerializedMonoBehaviour
    {
        public GridObjectProductDataContainer ProductDataContainer;
        public Dictionary<GridObjectProductDataContainer.ProductType, TextMeshProUGUI> ProductAmountTexts;

        private void OnEnable()
        {
            SetProductAmountTexts();
        }

        private void SetProductAmountTexts()
        {
            foreach (var productAmountText in ProductAmountTexts)
            {
                productAmountText.Value.text = ProductDataContainer.GetProductAmount(productAmountText.Key).ToString();
            }
        }
    }
}