using UnityEngine;
using DG.Tweening;

namespace SpriteAnimations
{
    public class SpriteItemAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float duration = 1f;
        [SerializeField] private float moveAmount = 0.5f;

        private Vector3 initialPosition;

        private void OnEnable()
        {
            CacheInitialPosition();
            PlayAnimation();
        }

        private void CacheInitialPosition()
        {
            initialPosition = transform.position;
        }

        private void PlayAnimation()
        {
            transform.DOMoveY(initialPosition.y + moveAmount, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine).SetLink(gameObject);
        }

        private void OnDisable()
        {
            ResetPosition();
        }

        private void ResetPosition()
        {
            transform.position = initialPosition;
        }
    }
}