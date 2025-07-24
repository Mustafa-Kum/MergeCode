using _Game.Scripts.Managers.Core;
using _Game.Scripts.UI.Buttons;

namespace _Game.Scripts._GameLogic.Logic.Debug
{
    public class DebugObjectGenerateUIButton : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.DebugEvents.OnDebugGenerateButtonClicked?.Invoke();
        }
    }
}