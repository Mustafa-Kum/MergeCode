using _Game.Scripts.Managers.Core;

namespace _Game.Scripts.UI.Buttons
{
    public class RetryLevelButton : ButtonBase
    {
        #region UNITY METHODS

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelStart -= OnLevelStart;
        }

        #endregion
        
        #region INHERITED METHODS

        protected override void OnClicked() {
        }
        
        #endregion

        #region PRIVATE METHODS
        
        private void OnLevelStart()
        {
            targetButton.interactable = true;
        }

        #endregion

    }
}
