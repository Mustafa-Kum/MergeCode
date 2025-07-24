using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.UI;
using _Game.Scripts._GameLogic.Logic.Grid;
using _Game.Scripts.Managers.Core;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Helper.Extensions.System;
using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.UI
{
    public class GridObjectGenerateUIController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private GridObjectGenerateUIData _uiData;
        [SerializeField] private TextMeshProUGUI _objectCountText;
        [SerializeField] private TextMeshProUGUI _currentTimeText;
        [SerializeField] private GameObject _rewardedObjectGenerateButton; 

        #endregion

        #region Private Fields

        private int _currentObjectCount;
        private double _currentTime;

        #endregion

        #region Unity Callbacks

        private void Start()
        {
            InitializeUI();
        }

        private void OnEnable()
        {
            EventManager.UIEvents.OnGridObjectGenerateUIClicked += HandleGenerateUIClicked;
            EventManager.UIEvents.GridObjectRewardedUIClicked += OnGridObjectRewardedUIClicked;
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnGridObjectGenerateUIClicked -= HandleGenerateUIClicked;
            EventManager.UIEvents.GridObjectRewardedUIClicked -= OnGridObjectRewardedUIClicked;
        }

        private void Update() 
        {
            UpdateTimer();
            UpdateButtonState();
        }

        #endregion

        #region Private Methods

        private void InitializeUI()
        {
            _currentObjectCount = _uiData.MaxObjectCount;
            _currentTime = _uiData.MaxTime;
            UpdateUI();
            CacheRuntimeData();
            UpdateButtonState();
        }

        private void UpdateTimer()
        {
            if (_currentTime <= 0) return;

            _currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (_currentTime <= 0)
            {
                AddToObjectCount(_uiData.ObjectIncreaseCount);
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _currentTime = _uiData.MaxTime;
            CacheRuntimeData();
        }

        private void OnGridObjectRewardedUIClicked()
        {
            AddToObjectCount(20);
        }

        private void AddToObjectCount(int amount)
        {
            _currentObjectCount = Mathf.Min(_currentObjectCount + amount, _uiData.MaxObjectCount);
            UpdateObjectCountUI();
            CacheRuntimeData();
            UpdateButtonState();
        }

        private void HandleGenerateUIClicked()
        {
            if (_currentObjectCount > 0)
            {
                GridTile emptyRandomTile = RuntimeGridDataCache.FindEmptyRandomTile();
                if(emptyRandomTile == null) return;
                _currentObjectCount--;
                UpdateObjectCountUI();
                CacheRuntimeData();
                UpdateButtonState();
            }
            else
            {
                TDebug.Log("No more objects to generate.");
            }
        }

        private void UpdateUI()
        {
            UpdateObjectCountUI();
            UpdateTimerUI();
        }

        private void UpdateObjectCountUI()
        {
            _objectCountText.text = $"{_currentObjectCount} / {_uiData.MaxObjectCount}";
        }

        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt((float)_currentTime / 60);
            int seconds = Mathf.FloorToInt((float)_currentTime % 60);
            _currentTimeText.text = $"{minutes:00}:{seconds:00}";
        }

        private void UpdateButtonState()
        {
            _rewardedObjectGenerateButton.SetActive(_currentObjectCount <= _uiData.MaxObjectCount / 3);
        }


        private void CacheRuntimeData()
        {
            RuntimeGridObjectGenerateDataCache.CurrentObjectCount = _currentObjectCount;
            RuntimeGridObjectGenerateDataCache.CurrentTime = _currentTime;
        }

        #endregion
    }
}
