using System;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts.UI.Listeners
{
    public class StartScreenPopUpExecutor : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        [SerializeField] private PlayerSavableData _playerSavableData;
        [SerializeField] private GameObject _popUp;
        private static bool _applicationEnter;

        #endregion

        #region PRIVATE VARIABLES

        private bool _canShowPopUp;

        #endregion

        #region PRIVATE METHODS

        private void ShowPopUp()
        {
            if (_applicationEnter == false)
            {
                if (_playerSavableData.LevelIndex > 0)
                {
                    _popUp.SetActive(_canShowPopUp);
                }
                
                _applicationEnter = true;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void AssignValue(int value)
        {
            _canShowPopUp = value switch
            {
                0 => false,
                1 => true,
                _ => true
            };
            
            ShowPopUp();
        }

        #endregion

    }
}