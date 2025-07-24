using System;
using UnityEngine;
using DG.Tweening;

namespace UIAnimations
{
    public class UITabAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform tab;
        [SerializeField] private float animationDuration = 0.5f;

        private Vector3 offScreenPosition = new Vector3(0, -1100, 0);
        private Vector3 onScreenPosition = Vector3.zero;

        private void OnEnable()
        {
            PlayAnimation(onScreenPosition, Ease.OutBack, true);
        }

        private void OnDisable()
        {
            ResetTabPosition();
        }

        public void PlayInBackAnimation()
        {
            PlayAnimation(offScreenPosition, Ease.InBack, false);
        }

        private void PlayAnimation(Vector3 targetPosition, Ease easeType, bool isOnEnable)
        {
            tab.DOLocalMoveY(targetPosition.y, animationDuration)
                .SetEase(easeType)
                .From(isOnEnable ? offScreenPosition.y : tab.localPosition.y)
                .OnComplete(() =>
                {
                    if (!isOnEnable)
                    {
                        tab.gameObject.SetActive(false);
                    }
                }).SetLink(tab.gameObject);
        }

        private void ResetTabPosition()
        {
            tab.localPosition = offScreenPosition;
        }
    }
}