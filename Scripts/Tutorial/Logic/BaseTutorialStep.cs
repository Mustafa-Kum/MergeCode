using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Tutorial.Logic
{
    public abstract class BaseTutorialStep : MonoBehaviour, ITutorialStep
    {
        #region SHARED VARIABLES

        public UnityEvent onStepActivated;
        
        public UnityEvent onStepDisabled;

        #endregion
        
        public virtual void ActivateStep()
        {
            onStepActivated?.Invoke();
        }

        public virtual void DisableStep()
        {
            onStepDisabled?.Invoke();
        }
    }
}