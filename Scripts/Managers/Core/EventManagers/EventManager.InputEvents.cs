using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class InputEvents
        {
            #region MouseInput

            public static UnityAction MouseDown;
            public static UnityAction MouseHold;
            public static UnityAction MouseUp;

            #endregion
        }
    }
}
