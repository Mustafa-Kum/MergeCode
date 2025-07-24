using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace _Game.Scripts.UI.Screens
{
    public class UIScaleTweenOnEnable : MonoBehaviour
    {
        #region Public Fields

        public Transform targetTransform;
        
        public float tweenDuration;
        
        #endregion

        #region Unity Methods
        
        private void OnEnable()
        {
            SetLocalScaleAtLoad();
            TweenToTarget();
        }

        #endregion

        #region Private Methods
        
        private void SetLocalScaleAtLoad()
        {
            targetTransform.DOScale(Vector3.zero, 0f);
        }

        private void TweenToTarget()
        {
            targetTransform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack);
        }

        #endregion
    }
}