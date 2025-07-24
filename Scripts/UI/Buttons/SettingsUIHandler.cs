using System;
using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using Lofelt.NiceVibrations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class SettingsUIHandler : MonoBehaviour
    {
        #region Public Variables

        public SettingsDataSO settingsData;
        public SoundToggleData soundToggleData;
        public VibrationToggleData vibrationToggleData;

        public float animationDuration = 0.3f;

        public UnityEvent InternalSettingsButtonEnabled;
        
        public UnityEvent InternalSettingsButtonDisabled;
        
        #endregion

        #region Unity Methods

        private void OnEnable() => InitializeToggles();

        #endregion

        #region Private Methods

        private void InitializeToggles()
        {
            soundToggleData.soundToggleRectTransform.anchoredPosition = settingsData.IsSoundEnabled ? soundToggleData.soundToggleOnPosition : soundToggleData.soundToggleOffPosition;
            vibrationToggleData.vibrationToggleRectTransform.anchoredPosition = settingsData.IsVibrationEnabled ? vibrationToggleData.vibrationToggleOnPosition : vibrationToggleData.vibrationToggleOffPosition;
            soundToggleData.soundToggleImage.color = settingsData.IsSoundEnabled ? Color.green : Color.red;
            vibrationToggleData.vibrationToggleImage.color = settingsData.IsVibrationEnabled ? Color.green : Color.red;
            
            OnSettingsButtonActivated();
            
            InternalSettingsButtonEnabled?.Invoke();
        }

        private void AnimateToggle(RectTransform toggleRectTransform, Vector2 targetPosition)
        {
            toggleRectTransform.DOAnchorPos(targetPosition, animationDuration).SetEase(Ease.InOutSine);
        }

        private void ColorChangeToggle(Image toggleImage, bool isOn)
        {
            Color targetColor = isOn ? Color.green : Color.red;
            toggleImage.DOColor(targetColor, animationDuration).SetEase(Ease.InOutSine);
        }


        #endregion

        #region Public Methods

        public void OnSoundToggleClicked()
        {
            bool isOn = !settingsData.IsSoundEnabled;
            settingsData.IsSoundEnabled = isOn;
            AnimateToggle(soundToggleData.soundToggleRectTransform, isOn ? soundToggleData.soundToggleOnPosition : soundToggleData.soundToggleOffPosition);
            EventManager.AudioEvents.VolumeChange?.Invoke(isOn ? 1f : 0f);
            ColorChangeToggle(soundToggleData.soundToggleImage, isOn);
            
            EventManager.AudioEvents.AudioEnabled?.Invoke(settingsData.IsSoundEnabled);
        }

        public void OnVibrationToggleClicked()
        {
            bool isOn = !settingsData.IsVibrationEnabled;
            settingsData.IsVibrationEnabled = isOn;
            AnimateToggle(vibrationToggleData.vibrationToggleRectTransform, isOn ? vibrationToggleData.vibrationToggleOnPosition : vibrationToggleData.vibrationToggleOffPosition);
            ColorChangeToggle(vibrationToggleData.vibrationToggleImage, isOn);
            
            if (isOn)
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);

        }

        private void OnSettingsButtonActivated()
        {
            EventManager.UIEvents.OnSettingsButtonActivated?.Invoke();
            
            InternalSettingsButtonEnabled?.Invoke();
        }
        
        public void OnSettingsButtonDeactivated()
        {
            EventManager.UIEvents.OnSettingsButtonDeactivated?.Invoke();
            
            InternalSettingsButtonDisabled?.Invoke();
        }

        #endregion
    }

    [Serializable]
    public struct SoundToggleData
    {
        public Image soundToggleImage;
        public RectTransform soundToggleRectTransform;
        public Vector2 soundToggleOnPosition;
        public Vector2 soundToggleOffPosition;
    }

    [Serializable]
    public struct VibrationToggleData
    {
        public Image vibrationToggleImage;
        public RectTransform vibrationToggleRectTransform;
        public Vector2 vibrationToggleOnPosition;
        public Vector2 vibrationToggleOffPosition;
    }
}
