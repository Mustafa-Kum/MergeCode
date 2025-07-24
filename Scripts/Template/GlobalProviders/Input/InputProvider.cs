using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts.Template.GlobalProviders.Input
{
    public abstract class InputProvider : MonoBehaviour
    {
        #region Private Variables
        
        private Coroutine _clickableCoroutine;
        
        private CoroutineService _coroutineService;

        #endregion
        
        #region Unity Methods

        private void Awake() => Initialize();

        private void Initialize() => _coroutineService = new CoroutineService(this);

        protected virtual void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += StartClickableEvents;
            EventManager.InGameEvents.LevelSuccess += StopClickableEvents;
            EventManager.InGameEvents.LevelFail += StopClickableEvents;
         
        }

        protected virtual void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= StartClickableEvents;
            EventManager.InGameEvents.LevelSuccess -= StopClickableEvents;
            EventManager.InGameEvents.LevelFail -= StopClickableEvents;
        }

        #endregion

        #region Private Methods

        private void StartClickableEvents() => _clickableCoroutine = _coroutineService.StartUpdateRoutine(ClickableEvents, () => true);

        private void StopClickableEvents() => _coroutineService.Stop(_clickableCoroutine);
        
        private void ClickableEvents()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnClickDown();
            }
            if (UnityEngine.Input.GetMouseButton(0))
            {
                OnClickHold();
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                OnClickUp();
                EventManager.InputEvents.MouseUp();
            }
        }

        #endregion

        #region Abstract Methods

        protected abstract void OnClickDown();

        protected abstract void OnClickHold();

        protected abstract void OnClickUp();

        #endregion
    }
}




