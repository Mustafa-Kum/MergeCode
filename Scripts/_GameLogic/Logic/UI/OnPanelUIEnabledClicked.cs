using _Game.Scripts.Managers.Core;
using _Game.Scripts.UI.Buttons;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class OnPanelUIEnabledClicked : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.UIEvents.OnAnyPanelEnabled?.Invoke();
        }
    }
}