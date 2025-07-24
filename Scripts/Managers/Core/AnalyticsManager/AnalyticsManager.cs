using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using _Game.Scripts.General.AnalyticsManager;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.ScriptableObjects.Saveable;
using Handler.Extensions;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core
{
    public class AnalyticsManager : MonoBehaviour
    {
        [SerializeField] private PlayerSavableData playerSavableData;
        private List<IAnalyticsService> _analyticsServices = new List<IAnalyticsService>();

        private void Awake()
        {
            Initialize();
            Subscribe();
        }

        private void Initialize()
        {
            // Initialize analytics services
        }

        private void Subscribe()
        {
            EventManager.InGameEvents.GameStarted += LogGameStart;
            EventManager.InGameEvents.LevelStart += LogLevelStart;
            EventManager.InGameEvents.LevelSuccess += LogLevelSuccess;
            EventManager.InGameEvents.LevelFail += LogLevelFail;
            EventManager.SaveEvents.DataLoaded += LogDataLoaded;
            EventManager.InGameEvents.LevelLoaded += LogLevelLoaded;
            EventManager.IAPEvents.PurchaseSuccess += LogPurchaseSuccess;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            EventManager.InGameEvents.GameStarted -= LogGameStart;
            EventManager.InGameEvents.LevelStart -= LogLevelStart;
            EventManager.InGameEvents.LevelSuccess -= LogLevelSuccess;
            EventManager.InGameEvents.LevelFail -= LogLevelFail;
            EventManager.SaveEvents.DataLoaded -= LogDataLoaded;
            EventManager.InGameEvents.LevelLoaded -= LogLevelLoaded;
            EventManager.IAPEvents.PurchaseSuccess -= LogPurchaseSuccess;
        }

        private void LogPurchaseSuccess(PurchaseEventArgs purchase)
        {
            var analyticData = new AnalyticData(new Dictionary<string, object>
            {
                {
                    "PurchaseInfo", purchase
                }
            });
            LogEvent("PurchaseSuccess", analyticData);
        }

        #region Logs

        private void LogGameStart()
        {
            LogEvent("GameStart");
        }

        private void LogLevelStart()
        {
            var analyticData = new AnalyticData(new Dictionary<string, object>
            {
                { "LevelIndex", playerSavableData.LevelIndex }
            });
            LogEvent("LevelStart", analyticData);
        }

        // Inside the AnalyticsManager class

        private void LogLevelSuccess()
        {
            var analyticData = new AnalyticData(
                new Dictionary<string, object>
                {
                    { "LevelIndex", playerSavableData.LevelIndex }
                }
            );
            LogEvent($"Levels:Level{playerSavableData.LevelIndex}:Success", analyticData);
        }
        
        private void LogRemainingCounts(float remainingCounts)
        {
            var analyticData = new AnalyticData(
                new Dictionary<string, object>
                {
                    { "LevelIndex", playerSavableData.LevelIndex }
                },
                remainingCounts // Pass remaining counts as event value
            );
            LogEvent($"Levels:Level{playerSavableData.LevelIndex}:RemainingCounts", analyticData);
        }

// Similarly adjust other methods as needed


        private void LogLevelFail()
        {
            LogEvent($"Levels:Level{playerSavableData.LevelIndex}:LevelFail");
        }

        private void LogDataLoaded()
        {
            LogEvent("DataLoaded");
        }

        private void LogLevelLoaded(GameObject levelGameObject)
        {
            // Assuming levelGameObject.name is the prefab name, and you want to use level index instead
            var analyticData = new AnalyticData(new Dictionary<string, object>
            {
                { "LevelIndex", playerSavableData.LevelIndex }
            });
            LogEvent("LevelLoaded", analyticData);
        }

        public void LogEvent(string eventName, AnalyticData eventData = null)
        {
            if (_analyticsServices == null)
            {
                Debug.LogWarning("Analytics Service not initialized.");
                return;
            }

            if (_analyticsServices.Count == 0)
            {
                Debug.LogWarning("No Analytics Services are initialized.");
                return;
            }

            foreach (var service in _analyticsServices)
            {
                service.LogEvent(eventName, eventData);
            }
        }

        #endregion
    }
}

public class AnalyticData
{
    public Dictionary<string, object> Parameters { get; set; }
    public float? EventValue { get; set; } // Optional event value

    public AnalyticData(Dictionary<string, object> parameters, float? eventValue = null)
    {
        Parameters = parameters;
        EventValue = eventValue;
    }
}
