using _Game.Scripts.Helper.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public abstract class ButtonBase : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] public Button targetButton;
        [SerializeField] private bool usePointerDownInsteadOfClick = false;

        public CoroutineService _coroutineService;

        private void Awake()
        {
            InitializeButton();
            
            _coroutineService = new CoroutineService(this);
        }

        private void InitializeButton()
        {
            if (targetButton == null)
            {
                targetButton = GetComponent<Button>();
            }

            if (usePointerDownInsteadOfClick)
            {
                // Remove all listeners from OnClick to disable it.
                targetButton.onClick.RemoveAllListeners();
            }
            else
            {
                targetButton.onClick.AddListener(HandleClick);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (usePointerDownInsteadOfClick)
            {
                HandleClick();
            }
        }

        public void HandleClick()
        {
            OnClicked();
        }

        protected abstract void OnClicked();
    }
}
