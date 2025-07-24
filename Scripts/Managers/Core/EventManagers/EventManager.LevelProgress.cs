using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class LevelProgressEvents
        {
            public static UnityAction<int> OnLevelProgressUpdated;
        }
    }
}