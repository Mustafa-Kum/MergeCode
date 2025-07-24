using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI.Texts
{
    public class SettingsBuildVersionTextAssigner : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        [SerializeField] private TextMeshProUGUI text;

        #endregion

        #region UNITY METHODS

        private void OnEnable()
        {
            UpdateBuildVersionText();
        }

        #endregion

        #region PRIVATE METHODS

        private void UpdateBuildVersionText()
        {
            text.text = $"v{Application.version}";
        }

        #endregion
    }
}