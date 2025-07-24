using _Game.Scripts.Managers.Core;
using _Game.Scripts.UI.Buttons;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class GridObjectRewardedGenerateUIClicked : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.UIEvents.GridObjectRewardedUIClicked?.Invoke();
        }
    }
}