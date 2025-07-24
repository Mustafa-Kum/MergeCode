using System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public abstract class BaseInputProvider : MonoBehaviour
    {
        private CoroutineService _coroutineService;
        private Vector3 _initialClickPosition;
        private bool _isDragging;
        private bool _mainInputEnabled = true;
        private const float DragThreshold = 10f;
        private Camera _mainCamera;
        private Coroutine _inputCoroutine;

        private void OnEnable()
        {
            _mainCamera = Camera.main;
            EventManager.UIEvents.OnAnyPanelEnabled += DisableInput;
            EventManager.UIEvents.OnAnyPanelDisabled += EnableInput;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnAnyPanelEnabled -= DisableInput;
            EventManager.UIEvents.OnAnyPanelDisabled -= EnableInput;
        }

        protected virtual void Awake()
        {
            _coroutineService = new CoroutineService(this);
            _inputCoroutine = _coroutineService.StartUpdateRoutine(HandleInput, () => _mainInputEnabled);
        }

        protected abstract void OnClick();
        protected abstract void OnDrag();
        protected abstract void OnRelease();
        
        protected Camera GetMainCamera() => _mainCamera;
        
        protected bool IsDragging() => _isDragging;
        
        private void DisableInput()
        {
            _mainInputEnabled = false;
            if (_inputCoroutine != null)
            {
                _coroutineService.Stop(_inputCoroutine);
                _inputCoroutine = null;
            }
            UnityEngine.Debug.Log("DisableInput");
        }

        private void EnableInput()
        {
            _mainInputEnabled = true;
            if (_inputCoroutine == null)
            {
                _inputCoroutine = _coroutineService.StartUpdateRoutine(HandleInput, () => _mainInputEnabled);
            }
            UnityEngine.Debug.Log("EnableInput");
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _initialClickPosition = Input.mousePosition;
                _isDragging = false;
                OnClick();
            }

            if (Input.GetMouseButton(0))
            {
                if (!_isDragging && Vector3.Distance(_initialClickPosition, Input.mousePosition) > DragThreshold)
                {
                    _isDragging = true;
                }

                if (_isDragging)
                {
                    OnDrag();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnRelease();
                _isDragging = false;
            }
        }
    }
}