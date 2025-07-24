using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class QuestPanelUIController : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;

        private void OnEnable()
        {
            EventManager.UIEvents.OnQuestRewardCollected += ClosePanel;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnQuestRewardCollected -= ClosePanel;
        }
        
        private void ClosePanel()
        {
            _closeButton.onClick.Invoke();
        }
    }
}