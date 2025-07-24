using _Game.Scripts.ScriptableObjects.Saveable;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI.Texts
{
    public class InGameLevelIndexTextAssigner : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private PlayerSavableData playerSavableData;
        private void Awake()
        {
            if(_textMeshProUGUI == null)
                _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }
        private void OnEnable()
        {
            _textMeshProUGUI.text = "Level " + "\n" + (playerSavableData.LevelIndex + 1);
        }
    }
}
