using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.UI;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Object = System.Object;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class CurrencyUIManager : SerializedMonoBehaviour
    {
        [SerializeField] private CurrencyValuesSO _currencyValuesSo;
        [SerializeField] private CurrencyParticleImageDataContainer _currencyParticleImageDataContainer;
        [SerializeField] private Canvas _canvas;
        public Dictionary<CurrencyType, TextMeshProUGUI> CurrencyTexts;
        private Camera _camera;

        private void OnEnable()
        {
            _camera = Camera.main;
            
            EventManager.Resource.CurrencyChanged += OnCurrencyChanged;
            EventManager.Resource.CurrencyCollected += OnCurrencyCollected;
        }

        private void OnDisable()
        {
            EventManager.Resource.CurrencyChanged -= OnCurrencyChanged;
            EventManager.Resource.CurrencyCollected -= OnCurrencyCollected;
        }

        private void Start()
        {
            foreach (var currencyText in CurrencyTexts)
            {
                currencyText.Value.text = _currencyValuesSo.GetValue(currencyText.Key).ToString();
            }
        }
        
        private void OnCurrencyChanged(CurrencyType type)
        {
            CurrencyTexts[type].text = _currencyValuesSo.GetValue(type).ToString();
        }
        
        private void OnCurrencyCollected(CurrencyType type, Vector3 position)
        {
            ParticleImage particle = _currencyParticleImageDataContainer.GetCurrencyParticle(type);
            if (particle == null) return;
            
            ParticleImage particleInstance = Instantiate(particle, transform.parent);
            var canvasPosition = ConvertObjectWorldPositionToCanvasPosition(position);
            var rectTransform = particleInstance.GetComponent<RectTransform>();
            particleInstance.attractorTarget = GetRectTransformFromType(type);
            
            rectTransform.anchoredPosition = canvasPosition;
            particleInstance.onLastParticleFinished.AddListener( () => Destroy(particleInstance.gameObject));
            particleInstance.Play();
        }
        
        private Vector2 ConvertObjectWorldPositionToCanvasPosition(Vector3 position)
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPosition, _canvas.worldCamera, out var canvasPosition);
            return canvasPosition;
        }
        
        private RectTransform GetRectTransformFromType(CurrencyType type)
        {
            return CurrencyTexts[type].GetComponent<RectTransform>();
        }
    }
}