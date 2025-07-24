using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.Saveable;
using Lofelt.NiceVibrations;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.HapticManager
{
    [RequireComponent(typeof(HapticController))]
    public class HapticManager : MonoBehaviour
    {
        #region Public Variables

        [SerializeField] private SettingsDataSO settingsData;
        
        [SerializeField] private HapticController hapticController;

        #endregion
        
        #region Unity Lifecycle Methods
        
        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += OnSuccess;
            EventManager.InGameEvents.LevelFail += OnFail;
            EventManager.InGameEvents.LevelStart += OnLevelStart;
        }

        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= OnSuccess;
            EventManager.InGameEvents.LevelFail -= OnFail;
            EventManager.InGameEvents.LevelStart -= OnLevelStart;
        }
        
        #endregion
        
        #region Event Callbacks
        
        private void OnLevelStart()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Selection);
        }
        
        private void OnSuccess()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Success);
        }
        
        private void OnFail()
        {
            if (settingsData.IsVibrationEnabled)
                hapticController.QueueHaptic(HapticPatterns.PresetType.Failure);
        }
        
        #endregion
    }
}
