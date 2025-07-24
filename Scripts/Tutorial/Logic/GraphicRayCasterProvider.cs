using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Tutorial.Logic
{
    public class GraphicRayCasterProvider : MonoBehaviour
    {
        #region PRIVATE VARIABLES

        private GraphicRaycaster _graphicRayCaster;

        #endregion

        #region UNITY METHODS

        private void Awake() => _graphicRayCaster = GetComponent<GraphicRaycaster>();

        private void OnEnable() => EventManager.InGameEvents.LevelStart += SendGraphicRayCaster;

        private void OnDisable() => EventManager.InGameEvents.LevelStart -= SendGraphicRayCaster;

        #endregion

        #region PRIVATE METHODS

        private void SendGraphicRayCaster() => EventManager.TutorialEvents.GraphicRayCasterSent?.Invoke(_graphicRayCaster);

        #endregion
    }
}