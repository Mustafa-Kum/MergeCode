using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace UIAnimations
{
    public class UIFadeAndMoveY : MonoBehaviour
    {
        [FoldoutGroup("Text Settings"), SerializeField, Tooltip("The TextMeshPro element to animate.")]
        private TextMeshProUGUI textToMoveAndFade;

        [FoldoutGroup("Image Settings"), SerializeField, Tooltip("The Image element to animate.")]
        private Image imageToMoveAndFade;

        [FoldoutGroup("Movement Settings"), SerializeField, Tooltip("The distance the UI element will move on the Y axis.")]
        private float moveDistance = 100f;

        [FoldoutGroup("Animation Settings"), SerializeField, Range(0.1f, 5f), Tooltip("Duration of the move and fade animation.")]
        [SuffixLabel("seconds", true)]
        private float duration = 1f;

        [FoldoutGroup("Fade Settings"), SerializeField, Range(0f, 1f), Tooltip("The target alpha value for the fade.")]
        private float fadeTo = 0f;

        [FoldoutGroup("Animation Settings"), SerializeField, Tooltip("Type of easing for the move and fade animation.")]
        private Ease easeType = Ease.InOutSine;

        [FoldoutGroup("Loop Settings"), SerializeField, Tooltip("Enable or disable looping of the animation.")]
        private bool loopAnimation = false;

        [FoldoutGroup("Loop Settings"), SerializeField, Tooltip("Time between looped animations if looping is enabled.")]
        [ShowIf("loopAnimation"), SuffixLabel("seconds", true)]
        private float delayBetweenLoops = 2f;
        
        private Tween animationTween;
        private Vector3 originalParentPosition;

        private void Awake()
        {
            originalParentPosition = transform.position;
        }

        private void OnEnable()
        {
            PlayFadeAndMoveAnimation();
        }

        private void OnDisable()
        {
            ResetPositionAndAlpha();
        }

        private void PlayFadeAndMoveAnimation()
        {
            originalParentPosition = transform.position;

            Sequence animationSequence = DOTween.Sequence();

            if (loopAnimation)
            {
                animationSequence.Append(transform.DOMoveY(originalParentPosition.y + moveDistance, duration)
                    .SetEase(easeType))
                    .Join(textToMoveAndFade.DOFade(fadeTo, duration))
                    .Join(transform.DOMoveY(originalParentPosition.y + moveDistance, duration)
                    .SetEase(easeType))
                    .Join(imageToMoveAndFade.DOFade(fadeTo, duration))
                    .AppendInterval(delayBetweenLoops)
                    .SetLoops(-1, LoopType.Restart);
            }
            else
            {
                animationSequence.Append(transform.DOMoveY(originalParentPosition.y + moveDistance, duration)
                    .SetEase(easeType))
                    .Join(textToMoveAndFade.DOFade(fadeTo, duration))
                    .Join(transform.DOMoveY(originalParentPosition.y + moveDistance, duration)
                    .SetEase(easeType))
                    .Join(imageToMoveAndFade.DOFade(fadeTo, duration));
            }

            animationTween = animationSequence.Play();
        }

        private void ResetPositionAndAlpha()
        {
            if (animationTween != null && animationTween.IsActive())
            {
                animationTween.Kill();
            }

            transform.position = originalParentPosition;
            
            textToMoveAndFade.alpha = 1f;
            
            imageToMoveAndFade.color = new Color(imageToMoveAndFade.color.r, imageToMoveAndFade.color.g, imageToMoveAndFade.color.b, 1f);
        }
    }
}
