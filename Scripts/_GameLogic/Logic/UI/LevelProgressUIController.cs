using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Data.UI;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class LevelProgressUIController : MonoBehaviour
    {
        [SerializeField] private CurrencyParticleImageDataContainer _currencyParticleImageDataContainer;
        [SerializeField] private Slider _levelProgressSlider;
        [SerializeField] private TextMeshProUGUI _levelProgressSliderText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private RectTransform _attractorTarget;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Camera _camera;
        
        private int _level;
        private int _levelProgress;
        private int _levelProgressMax = 100;

        private void Awake()
        {
            InitializeLevel();
        }

        private void OnEnable()
        {
            EventManager.GridEvents.OnGridObjectMerge += OnMergeGenerateLevelProgressParticle;
            EventManager.LevelProgressEvents.OnLevelProgressUpdated += UpdateLevelProgress;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnGridObjectMerge -= OnMergeGenerateLevelProgressParticle;
            EventManager.LevelProgressEvents.OnLevelProgressUpdated -= UpdateLevelProgress;
        }

        private void InitializeLevel()
        {
            _level = 1;
            _levelProgress = 0;
            UpdateUI();
        }

        private void UpdateLevelProgress(int xp)
        {
            _levelProgress += xp;
            
            while (_levelProgress >= _levelProgressMax)
            {
                _levelProgress -= _levelProgressMax;
                LevelUp();
            }

            UpdateUI();
        }

        private void LevelUp()
        {
            _level++;
            _levelProgressMax += 10;
        }

        private void UpdateUI()
        {
            _levelText.text = $"{_level}";
            _levelProgressSlider.maxValue = _levelProgressMax;
            _levelProgressSlider.value = _levelProgress;
            _levelProgressSliderText.text = $"{_levelProgress}/{_levelProgressMax}";
        }
        
        private void OnMergeGenerateLevelProgressParticle(GridTile tile)
        {
            var particle = _currencyParticleImageDataContainer.GetMergeParticle();
            var particleInstance = InstantiateParticleInstance(particle, tile);
            PrepareParticleAnimation(particleInstance);
            particleInstance.Play();
        }
        
        private ParticleImage InstantiateParticleInstance(ParticleImage particle, GridTile tile)
        {
            var particleInstance = Instantiate(particle, transform.parent);
            var canvasPosition = ConvertObjectWorldPositionToCanvasPosition(tile.GetPosition());

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

                sequence.Append(_attractorTarget.DOPunchScale(Vector3.one * 0.3f, 0.1f, 1, 0.5f))
                    .SetLink(particleInstance.gameObject);
                sequence.Append(_attractorTarget.DOScale(Vector3.one, 0.1f));
                sequence.AppendCallback(() => { Destroy(particleInstance.gameObject); });
            });
        }
    }
}