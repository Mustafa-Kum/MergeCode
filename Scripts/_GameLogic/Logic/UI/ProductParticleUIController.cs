using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Data.UI;
using _Game.Scripts.Managers.Core;
using AssetKits.ParticleImage;
using DG.Tweening;
using UnityEngine;
using Product = _Game.Scripts._GameLogic.Logic.Grid.Currencies.Product;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class ProductParticleUIController : MonoBehaviour
    {
        [SerializeField] private CurrencyParticleImageDataContainer _currencyParticleImageDataContainer;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _attractorTarget;

        private void OnEnable() => EventManager.ProductEvents.OnProductGenerateParticleImage += ProductGenerateParticleImage;

        private void OnDisable() => EventManager.ProductEvents.OnProductGenerateParticleImage -= ProductGenerateParticleImage;

        private void ProductGenerateParticleImage(GridObjectProductDataContainer.ProductType productType,
            List<Product> products)
        {
            foreach (var product in products)
            {
                var particle = _currencyParticleImageDataContainer.GetProductParticle(productType);
                if (particle == null) continue;

                UnityEngine.Debug.Log("ProductParticleUIController: ProductGenerateParticleImage: " + productType);

                var particleInstance = InstantiateParticleInstance(particle, product);
                PrepareParticleAnimation(particleInstance);
                particleInstance.Play();
            }
        }

        private ParticleImage InstantiateParticleInstance(ParticleImage particle, Product product)
        {
            var particleInstance = Instantiate(particle, transform);
            var canvasPosition = ConvertObjectWorldPositionToCanvasPosition(product.GetPosition());

            var rectTransform = particleInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = canvasPosition;

            particleInstance.attractorTarget = _attractorTarget;

            return particleInstance;
        }
        
        private Vector2 ConvertObjectWorldPositionToCanvasPosition(Vector3 position)
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, screenPosition,
                _canvas.worldCamera, out var canvasPosition);
            return canvasPosition;
        }

        private void PrepareParticleAnimation(ParticleImage particleInstance)
        {
            particleInstance.onLastParticleFinished.AddListener(() =>
            {
                var sequence = DOTween.Sequence();

                sequence.Append(_attractorTarget.DOPunchScale(Vector3.one * 0.2f, 0.05f, 1, 0.5f))
                    .SetLink(particleInstance.gameObject);
                sequence.Append(_attractorTarget.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.1f));
                sequence.AppendCallback(() => { Destroy(particleInstance.gameObject); });
            });
        }
    }
}