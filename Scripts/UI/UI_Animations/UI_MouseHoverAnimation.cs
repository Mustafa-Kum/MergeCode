using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace UIAnimations
{
    public class UI_MouseHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [BoxGroup("UI Elements")]
        [SerializeField] private RectTransform iconTransform;

        [BoxGroup("UI Elements")]
        [SerializeField] private RectTransform backgroundTransform;

        [BoxGroup("Animation Settings")]
        [MinValue(0.1f), MaxValue(3f)]
        [LabelText("Icon Scale Multiplier")]
        [SerializeField] private float iconScaleMultiplier = 1.2f;

        [BoxGroup("Animation Settings")]
        [MinValue(0.1f), MaxValue(3f)]
        [LabelText("Background Scale Multiplier")]
        [SerializeField] private float backgroundScaleMultiplier = 1.1f;

        [BoxGroup("Animation Settings")]
        [MinValue(0f), MaxValue(50f)]
        [LabelText("Move Y Distance")]
        [SerializeField] private float moveYDistance = 10f;

        [BoxGroup("Animation Settings")]
        [MinValue(0.1f), MaxValue(2f)]
        [LabelText("Animation Duration")]
        [SerializeField] private float animationDuration = 0.2f;
        
        private Vector3 originalIconScale;
        private Vector3 originalIconPosition;
        private Vector3 originalBackgroundScale;
        
        private void CacheOriginalTransforms()
        {
            originalIconScale = iconTransform.localScale;
            originalIconPosition = iconTransform.localPosition;
            originalBackgroundScale = backgroundTransform.localScale;
        }

        private void Start()
        {
            CacheOriginalTransforms();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayAnimation(
                iconTransform, 
                originalIconScale * iconScaleMultiplier, 
                originalIconPosition.y + moveYDistance, 
                Ease.OutBack);

            PlayAnimation(
                backgroundTransform, 
                originalBackgroundScale * backgroundScaleMultiplier, 
                null, 
                Ease.OutBack);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayAnimation(
                iconTransform, 
                originalIconScale, 
                originalIconPosition.y, 
                Ease.InBack);

            PlayAnimation(
                backgroundTransform, 
                originalBackgroundScale, 
                null, 
                Ease.InBack);
        }

        private void PlayAnimation(RectTransform target, Vector3 targetScale, float? targetYPosition, Ease easeType)
        {
            target.DOScale(targetScale, animationDuration).SetEase(easeType);

            if (targetYPosition.HasValue)
            {
                target.DOLocalMoveY(targetYPosition.Value, animationDuration).SetEase(easeType);
            }
        }
    }
}