using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

public class InterstitialAdManager : MonoBehaviour
{
    #region PUBLIC VARIABLES

    [SerializeField] private IAPBundleValuesSO _bundleValuesSo;
    [SerializeField] private PlayerSavableData _playerSavableData;

    public static InterstitialAdManager Instance;

    #endregion

    #region PRIVATE VARIABLES

    private float _timeSinceLastLevelEnd = 0f;
    private bool _timerActive = false;
    private float _requiredTime;
    private bool _canInterstialShow;

    #endregion


    #region Unity Lifecycle

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make the instance persistent across scenes
            Debug.Log("DontDestroyOnLoad: InterstitialAdManager");
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }
    }

    private void OnEnable()
    {
        EventManager.InGameEvents.LevelSuccess += OnLevelEnd;
        EventManager.InGameEvents.LevelFail += OnLevelEnd;
    }

    private void OnDisable()
    {
        EventManager.InGameEvents.LevelSuccess -= OnLevelEnd;
        EventManager.InGameEvents.LevelFail -= OnLevelEnd;
    }

    private void Update()
    {
        if (!_timerActive) return;
        _timeSinceLastLevelEnd += Time.deltaTime;
    }

    #endregion

    #region Event Responses

    private void OnLevelEnd()
    {
        if (!_timerActive) // Start timer after the first level end
        {
            _timeSinceLastLevelEnd = 0f;
            _timerActive = true;
        }
        else
        {
            CheckAndShowInterstitial();
        }
    }

    #endregion

    #region Advertisement Management

    public void CheckAndShowInterstitial()
    {
        if (_bundleValuesSo.NoAdsPurchased)
        {
            return; 
        }

        if (_playerSavableData.LevelIndex == 0) return;

        if (!_canInterstialShow) return;

        if (_timeSinceLastLevelEnd >= _requiredTime)
        {
            EventManager.AdEvents.ShowInterstitial.Invoke();
            Debug.Log("Showing Interstitial Ad");
            ResetAdTimer();
        }
    }

    private void ResetAdTimer()
    {
        _timeSinceLastLevelEnd = 0f;
    }

    #endregion

    public void AssignValue(int value)
    {
        _requiredTime = value switch
        {
            1 => 30f,
            2 => 60f,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };

        _canInterstialShow = value != 0;
    }
}