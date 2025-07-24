using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class CubeHintSettingsAnimation : MonoBehaviour
    {
        [SerializeField] private Image[] _cubeIcons;
        private List<RectTransform> _cubeIconRects = new List<RectTransform>();
        private List<Vector3> _originalScales = new List<Vector3>();
        private List<Vector3> _originalPositions = new List<Vector3>();
        private bool _isTransformed = false;

        private void Awake()
        {
            foreach (Image icon in _cubeIcons)
            {
                RectTransform rectTransform = icon.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    _cubeIconRects.Add(rectTransform);
                    _originalPositions.Add(rectTransform.localPosition);
                    _originalScales.Add(rectTransform.localScale);
                }
            }
        
            _isTransformed = false;
            _cubeIconRects.ForEach(rect => rect.DOLocalMove(Vector3.zero, 0f));
            _cubeIconRects.ForEach(rect => rect.DOScale(Vector3.zero, 0f));
        }
    
        public void ToggleIconsTransform()
        {
            for (int i = 0; i < _cubeIconRects.Count; i++)
            {
                if (_isTransformed)
                {
                    _cubeIconRects[i].DOLocalMove(_originalPositions[i], 0.5f);
                    _cubeIconRects[i].DOScale(_originalScales[i], 0.5f);
                }
                else
                {
                    _cubeIconRects[i].DOLocalMove(Vector3.zero, 0.5f);
                    _cubeIconRects[i].DOScale(Vector3.zero, 0.5f);
                }
            }
            _isTransformed = !_isTransformed;
        }
    }
}
