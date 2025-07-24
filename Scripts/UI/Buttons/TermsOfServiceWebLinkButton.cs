using UnityEngine;

namespace _Game.Scripts.UI.Buttons
{
    public class TermsOfServiceWebLinkButton : ButtonBase
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private string termsOfServiceURL = "https://voyagerapplication.com/terms-of-service/";

        #endregion
        
        #region PRIVATE METHODS

        protected override void OnClicked() => Application.OpenURL(termsOfServiceURL);

        #endregion
    }
}