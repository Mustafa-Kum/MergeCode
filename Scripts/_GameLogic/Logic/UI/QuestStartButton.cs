using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class QuestStartButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buttonText;

        private void OnEnable()
        {
            SetText("Start Quest");
        }

        private void OnDisable()
        {
            SetText("Start Quest");
        }


        private void SetText(string text)
        {
            _buttonText.text = text;
        }
    }
}