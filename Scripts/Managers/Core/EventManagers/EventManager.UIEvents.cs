using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Controller;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public partial class EventManager
    {
        public static class UIEvents
        {
            #region UI

            public static UnityAction OnSettingsButtonActivated;
            public static UnityAction OnSettingsButtonDeactivated;
            public static UnityAction<bool> PurchaseRestoredResult;
            public static UnityAction OnGridObjectGenerateUIClicked;
            public static UnityAction<Quest> OnQuestSelected;
            public static UnityAction OnQuestRewardCollected;
            public static UnityAction<List<Quest.QuestReward>> OnCurrencyRewardCollected;
            
            public static UnityAction OnAnyPanelEnabled;
            public static UnityAction OnAnyPanelDisabled;
            
            public static UnityAction GridObjectRewardedUIClicked;

            #endregion
        }
    }
}