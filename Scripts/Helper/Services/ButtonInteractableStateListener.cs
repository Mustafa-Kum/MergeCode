using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts.Helper.Services
{
    [RequireComponent(typeof(Button))]
    public class ButtonInteractableStateListener : MonoBehaviour
    {
        public UnityEvent onInteractableEnabled;
        public UnityEvent onInteractableDisabled;

        private Button _button;
        private bool _lastInteractableState;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _lastInteractableState = _button.interactable;
            
            SetBeginningInteractableState();
        }
        
        private void SetBeginningInteractableState()
        {
            if (_lastInteractableState)
            {
                onInteractableEnabled.Invoke();
            }
            else
            {
                onInteractableDisabled.Invoke();
            }
        }

        private void Update()
        {
            if (_button.interactable != _lastInteractableState)
            {
                _lastInteractableState = _button.interactable;

                if (_lastInteractableState)
                {
                    onInteractableEnabled.Invoke();
                }
                else
                {
                    onInteractableDisabled.Invoke();
                }
            }
        }
    }
}