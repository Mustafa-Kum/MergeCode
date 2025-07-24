using _Game.Scripts._GameLogic.Logic.Grid;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class TimerEvents
        {
            public static UnityAction<GridObjectSleepController, bool> UIOnTimerBegan;
            public static UnityAction<GridObjectSleepController, bool> UIOnTimerEnded;
            public static UnityAction<GridObjectSleepController> UIActivated;
        }
    }
}