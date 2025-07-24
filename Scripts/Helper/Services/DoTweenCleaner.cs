using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Helper.Services
{
    public class DoTweenCleaner : MonoBehaviour
    {
        #region Unity Methods

        void OnApplicationQuit()
        {
            CleanupDOTween();
        }

        void OnDestroy()
        {
            CleanupDOTween();
        }

        #endregion

        #region Private Methods

        private void CleanupDOTween()
        {
            DOTween.KillAll();
            DOTween.Clear();
        }

        #endregion
    }
}