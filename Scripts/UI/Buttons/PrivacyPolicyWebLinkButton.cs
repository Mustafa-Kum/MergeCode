using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class PrivacyPolicyWebLinkButton : ButtonBase
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private string privacyPolicyURL = "https://voyagerapplication.com/privacy-policy/";

        #endregion
        
        #region PRIVATE METHODS

        protected override void OnClicked() => Application.OpenURL(privacyPolicyURL);

        #endregion
    }
}