namespace _Game.Scripts.Helper.Services
{
    public class LevelSuccessHandler
    {
        private bool _isLevelSuccessTriggered = false;
        
        public void TriggerLevelSuccess(float delay)
        {
            if (!_isLevelSuccessTriggered)
            {
                GameEventService.DelayedTriggerLevelSuccess(delay);
                _isLevelSuccessTriggered = true;
            }
        }
    }
}