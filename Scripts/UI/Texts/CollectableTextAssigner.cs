using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI.Texts
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CollectableCounterText : MonoBehaviour
    {
        [SerializeField]
        private CurrencyType textAssigner;
        [SerializeField]
        private CurrencyValuesSO currencyValuesSo;
        private TextMeshProUGUI textMesh;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
         
            UpdateText(textAssigner);
        }
        
        private void OnEnable()
        {
            EventManager.Resource.CurrencyChanged += UpdateText;
        }

        private void OnDisable()
        {
            EventManager.Resource.CurrencyChanged -= UpdateText;
        }
        
        private void UpdateText(CurrencyType currencyType)
        {
            if (currencyType == textAssigner)
            {
                int value = currencyValuesSo.GetValue(currencyType);
            
                textMesh.text = value.ToString();
            }
        }
    }
}
