using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace UIAnimations
{
    public class UIScaleAnimation : MonoBehaviour
    {
        [FoldoutGroup("Scale Settings"), SerializeField, Tooltip("The initial scale of the UI element.")]
        private Vector3 initialScale = Vector3.zero;

        [FoldoutGroup("Scale Settings"), SerializeField, Tooltip("The target scale of the UI element.")]
        private Vector3 targetScale = Vector3.one;

        [FoldoutGroup("Animation Settings"), SerializeField, Range(0.1f, 5f), Tooltip("Duration of the scale animation.")]
        [SuffixLabel("seconds", true)]
        private float duration = 0.5f;

        [FoldoutGroup("Animation Settings"), SerializeField, Tooltip("Type of easing for the scale animation.")]
        private Ease easeType = Ease.InOutBack;

        [FoldoutGroup("Loop Settings"), SerializeField, Tooltip("Enable or disable looping of the scale animation.")]
        private bool loopAnimation = false;

        [FoldoutGroup("Loop Settings"), SerializeField, Tooltip("Time between looped animations if looping is enabled.")]
        [ShowIf("loopAnimation"), SuffixLabel("seconds", true)]
        private float delayBetweenLoops = 2f;

        private Tween scaleTween;

        private void OnEnable()
        {
            PlayScaleAnimation();
        }

        private void OnDisable()
        {
            ResetScale();
        }

        private void PlayScaleAnimation()
        {
            transform.localScale = initialScale;

            Sequence scaleSequence = DOTween.Sequence();
            
            if (loopAnimation)
            {
                scaleSequence.Append(transform.DOScale(targetScale, duration)
                    .SetEase(easeType)
                    .SetLoops(2, LoopType.Yoyo));
                
                scaleSequence.AppendInterval(delayBetweenLoops)
                             .SetLoops(-1, LoopType.Restart);
            }
            else
            {
                scaleSequence.Append(transform.DOScale(targetScale, duration)
                    .SetEase(easeType));
            }

            scaleTween = scaleSequence.Play();
        }

        private void ResetScale()
        {
            if (scaleTween != null && scaleTween.IsActive())
            {
                scaleTween.Kill();
            }
            transform.localScale = initialScale;
        }
    }
}
