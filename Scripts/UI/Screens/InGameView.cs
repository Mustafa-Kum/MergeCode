using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.UI.Screens
{
    public class InGameView : MonoBehaviour
    {
        #region Public Variables

        public UnityEvent OnInGameViewDisabled;

        #endregion
        
        #region Unity Methods

        private void OnDisable() => OnInGameViewDisabled?.Invoke();

        #endregion
    }
}