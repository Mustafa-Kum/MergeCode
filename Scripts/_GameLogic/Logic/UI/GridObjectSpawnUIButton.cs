using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.UI.Buttons;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class GridObjectSpawnUIButton : ButtonBase
    {
        protected override void OnClicked()
        {
            if(RuntimeGridObjectGenerateDataCache.GetCurrentObjectCount() <= 0) return;
            EventManager.UIEvents.OnGridObjectGenerateUIClicked?.Invoke();
        }
    }
}