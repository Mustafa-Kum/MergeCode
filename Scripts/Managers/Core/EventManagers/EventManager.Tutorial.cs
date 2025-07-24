using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts.Managers.Core
{
    public static partial class EventManager
    {
        public static class TutorialEvents
        {
            public static UnityAction<GraphicRaycaster> GraphicRayCasterSent;
            
            public static UnityAction<float> LevelStartTutorialCaching;
        }
    }
}