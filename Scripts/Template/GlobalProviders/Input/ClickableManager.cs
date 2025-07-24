using System.Collections.Generic;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.Template.GlobalProviders.Input
{
    public class ClickableManager : InputProvider
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;

        #endregion

        #region PRIVATE VARIABLES

        private LayerMask _ignoreRaycastLayer = 1 << 2;

        #endregion

        #region UNITY METHODS

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.TutorialEvents.GraphicRayCasterSent += ReceiveGraphicRaycaster;
            
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.TutorialEvents.GraphicRayCasterSent -= ReceiveGraphicRaycaster;
        }

        #endregion
        
        #region Inherited Methods

        protected override void OnClickDown()
        {
            if (IsPointerOverUIElement(UnityEngine.Input.mousePosition))
            {
                return;
            }

            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~_ignoreRaycastLayer.value))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClickedDown();
                }
            }
        }

        protected override void OnClickHold()
        {
            if (IsPointerOverUIElement(UnityEngine.Input.mousePosition))
            {
                return;
            }
            
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~_ignoreRaycastLayer.value))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClickedHold();
                }
            }
        }

        protected override void OnClickUp()
        {
            if (IsPointerOverUIElement(UnityEngine.Input.mousePosition))
            {
                return;
            }
            
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, ~_ignoreRaycastLayer.value))
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClickedUp();
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        private bool IsPointerOverUIElement(Vector2 screenPosition)
        {
            if (_eventSystem == null) return false;
            if (_graphicRaycaster == null) return false;
            
            PointerEventData eventDataCurrentPosition = new PointerEventData(_eventSystem)
            {
                position = screenPosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        
        private void ReceiveGraphicRaycaster(GraphicRaycaster graphic)
        {
            _graphicRaycaster = graphic;
        }

        #endregion
    }
}