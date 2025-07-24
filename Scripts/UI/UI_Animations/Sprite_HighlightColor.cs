using UnityEngine;
using DG.Tweening;

namespace SpriteAnimations
{
    public class Sprite_HighlightAnimation : MonoBehaviour
    {
        [Header("Highlight Settings")]
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private float duration = 1f;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Tween highlightTween;
        private bool isAnimating = false;

        private void OnEnable()
        {
            PlayHighlightAnimation();
        }

        private void PlayHighlightAnimation()
        {
            if (spriteRenderer != null && !isAnimating)
            {
                isAnimating = true;
                highlightTween = spriteRenderer.DOColor(highlightColor, duration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
                    .OnKill(() => isAnimating = false);
            }
        }
    }
}