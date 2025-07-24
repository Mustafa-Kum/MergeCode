using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts.Managers.Core
{
    public class LevelDebuggerActivator : MonoBehaviour
    {
        #region INSPECTOR VARIABLES
        
        [SerializeField] private GameObject levelDebuggerPanel;
        
        [SerializeField] private int tapCountToActivate = 5;
        
        [SerializeField] private UnityEvent onDebuggerActivated;

        [SerializeField] private Button _debuggerActivatorButton;

        #endregion
        
        #region PRIVATE VARIABLES
        
        private int _tapCount;
        
        #endregion
        
        #region UNITY METHODS
        
        private void OnEnable()
        {
            _debuggerActivatorButton.onClick.AddListener(OnClick);
        }

        #endregion

        #region PRIVATE METHODS

        private void OnClick()
        {
            _tapCount++;
            if (_tapCount >= tapCountToActivate)
            {
                onDebuggerActivated?.Invoke();
                _tapCount = 0;
            }
        }

        #endregion
    }
}