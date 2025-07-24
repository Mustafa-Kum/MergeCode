using System;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Helper.Services;
using _Game.Scripts.Managers.Core;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class CameraDragController : BaseInputProvider
    {
        [SerializeField] private CameraDragSettings _cameraDragSettings;
        private Vector3 _dragOrigin;
        private Vector3 _targetPosition;
        private Vector3 _velocity = Vector3.zero;
        private Camera _camera;
        private CoroutineService _coroutineService;
        private Coroutine _updateMovementRoutine;
        private bool _isObjectDragging;
        private float _targetZoom;

        protected override void Awake()
        {
            base.Awake();
            _coroutineService = new CoroutineService(this);
        }

        private void Start()
        {
            _camera = Camera.main;
            TDebug.Assert(_camera != null, "Main camera is not found in the scene.");
            _targetPosition = transform.position;
            System.Diagnostics.Debug.Assert(_camera != null, nameof(_camera) + " != null");
            _targetZoom = _camera.orthographicSize;
            _updateMovementRoutine = _coroutineService.StartUpdateRoutine(UpdateMovement, () => true);
        }

        private void OnEnable()
        {
            EventManager.GridEvents.OnGridTileClicked += OnGridTileClicked;
            EventManager.GridEvents.OnGridTileReleased += OnGridTileReleased;
            EventManager.UIEvents.OnAnyPanelEnabled += DisableInput;
            EventManager.UIEvents.OnAnyPanelDisabled += EnableInput;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnGridTileClicked -= OnGridTileClicked;
            EventManager.GridEvents.OnGridTileReleased -= OnGridTileReleased;
            EventManager.UIEvents.OnAnyPanelEnabled -= DisableInput;
            EventManager.UIEvents.OnAnyPanelDisabled -= EnableInput;
        }

        private void EnableInput()
        {
            _updateMovementRoutine = _coroutineService.StartUpdateRoutine(UpdateMovement, () => true);
        }

        private void DisableInput()
        {
            _coroutineService.Stop(_updateMovementRoutine);
        }

        protected override void OnClick()
        {
            if (_isObjectDragging) return;
            _dragOrigin = Input.mousePosition;
            _targetPosition = transform.position;
        }

        protected override void OnDrag()
        {
            if (_isObjectDragging) return;
            ProcessDrag(_dragOrigin);
        }
        
        protected override void OnRelease()
        {
            if (_isObjectDragging) return;
            var additionalTarget = _targetPosition;
            _targetPosition = AdjustPositionWithinLimits(additionalTarget);
        }
        
        private void OnGridTileClicked()
        {
            _isObjectDragging = true;
            _coroutineService.Stop(_updateMovementRoutine);
        }
        
        private void OnGridTileReleased(bool hasObject)
        {
            _updateMovementRoutine = _coroutineService.StartUpdateRoutine(UpdateMovement, () => true);
            _isObjectDragging = false;
        }

        private void ProcessDrag(Vector3 dragOrigin)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            var movement = CalculateMovement(currentMousePosition, dragOrigin);

            _targetPosition += movement;
            _targetPosition = AdjustPositionWithinLimits(_targetPosition);

            _dragOrigin = currentMousePosition;
        }

        private Vector3 CalculateMovement(Vector3 currentMousePosition, Vector3 dragOrigin)
        {
            Vector3 difference = currentMousePosition - dragOrigin;
            Vector3 move = GetMovement(difference);

            Vector3 combinedMovement = _camera.transform.right * move.x + _camera.transform.forward * move.z;
            combinedMovement.y = 0;

            return combinedMovement;
        }

        private Vector3 GetMovement(Vector3 difference)
        {
            return new Vector3(-difference.x * _cameraDragSettings.DragSpeed, 0,
                -difference.y * _cameraDragSettings.DragSpeed);
        }

        private Vector3 AdjustPositionWithinLimits(Vector3 targetPosition)
        {
            return new Vector3(
                Mathf.Clamp(targetPosition.x, _cameraDragSettings.XLimits.x, _cameraDragSettings.XLimits.y),
                targetPosition.y,
                Mathf.Clamp(targetPosition.z, _cameraDragSettings.ZLimits.x, _cameraDragSettings.ZLimits.y)
            );
        }

        private void UpdateMovement()
        {
            var smoothTime = _cameraDragSettings.SmoothTime;
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);
            transform.position = new Vector3(transform.position.x, _targetPosition.y, transform.position.z);
            
            // Handle smooth zoom
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoom, ref _velocity.y, _cameraDragSettings.ZoomSmoothTime);
        }

        private void Update()
        {
            HandleZoom();
        }

        private void HandleZoom()
        {
            // Mouse scroll wheel zoom
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f)
            {
                _targetZoom -= scrollInput * _cameraDragSettings.ZoomSpeed;
                _targetZoom = Mathf.Clamp(_targetZoom, _cameraDragSettings.MinZoom, _cameraDragSettings.MaxZoom);
            }

            // Pinch zoom for mobile
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                _targetZoom += deltaMagnitudeDiff * _cameraDragSettings.PinchZoomSpeed;
                _targetZoom = Mathf.Clamp(_targetZoom, _cameraDragSettings.MinZoom, _cameraDragSettings.MaxZoom);
            }
        }

        [Serializable]
        private struct CameraDragSettings
        {
            public float DragSpeed;
            public float SmoothTime;
            public Vector2 XLimits;
            public Vector2 ZLimits;
            public float ZoomSpeed;
            public float PinchZoomSpeed;
            public float ZoomSmoothTime;
            public float MinZoom;
            public float MaxZoom;
        }
    }
}
