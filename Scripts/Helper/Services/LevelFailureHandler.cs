namespace _Game.Scripts.Helper.Services
{
    public class LevelFailureHandler
    {
        private bool _isLevelFailTriggered = false;

        public void TriggerLevelFail(float delay)
        {
            if (!_isLevelFailTriggered)
            {
                GameEventService.DelayedTriggerLevelFail(delay);
                _isLevelFailTriggered = true;
            }
        }
    }
}