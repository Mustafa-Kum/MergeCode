using System.Collections.Generic;
using _Game.Scripts.Audio;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.ScriptableObjects.Predefined;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class AudioManager : SerializedMonoBehaviour
    {
        #region Public Variables

        public AudioSource audioSource;

        [ShowInInspector] public Dictionary<SoundType, AudioClipData> audioClipsData;

        public SettingsDataSO settingsData;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
            EventManager.AudioEvents.AudioPlay += PlayAudio;
            EventManager.AudioEvents.VolumeChange += SetAudioVolume;
            EventManager.AudioEvents.AudioEnabled += UpdateSettingsData;
            EventManager.InGameEvents.LevelStart += InitializeAudioSettings;
        }

        private void UnsubscribeFromEvents()
        {
            EventManager.AudioEvents.AudioPlay -= PlayAudio;
            EventManager.AudioEvents.VolumeChange -= SetAudioVolume;
            EventManager.AudioEvents.AudioEnabled -= UpdateSettingsData;
            EventManager.InGameEvents.LevelStart -= InitializeAudioSettings;
        }

        private void InitializeAudioSettings()
        {
            UpdateSettingsData(settingsData.IsSoundEnabled);
        }

        private void UpdateSettingsData(bool isEnabled)
        {
            audioSource.mute = !isEnabled;
        }

        private void PlayAudio(SoundType audioClipId, bool isRandomPitch, bool isOneShot)
        {
            if (!audioClipsData.ContainsKey(audioClipId))
            {
                Debug.LogError($"Audio clip ID {audioClipId} not found.");
                return;
            }

            var clipData = audioClipsData[audioClipId];
            
            SetAudioPitch(clipData, isRandomPitch);
            SetAudioClipToSource(clipData.AudioClip);
            SetAudioVolume(clipData.Volume);
            SetAudioLooping(clipData.ShouldLoop);
               

            if (isOneShot)
            {
                audioSource.PlayOneShot(clipData.AudioClip);
            }
            else
            {
                audioSource.Play();
            }
        }

        private void SetAudioVolume(float initialVolume = 1f)
        {
            audioSource.volume = initialVolume;
        }
        
        private void SetAudioLooping(bool shouldLoop)
        {
            audioSource.loop = shouldLoop;
        }

        private void SetAudioPitch(AudioClipData clipData, bool isRandomPitch)
        {
            if (isRandomPitch) 
            {
                audioSource.pitch = Random.Range(clipData.Pitch - 0.1f, clipData.Pitch + 0.1f);
            }
            else
            {
                audioSource.pitch = clipData.Pitch;
            }
        }
        
        private AudioClip GetAudioClip(SoundType audioClipId)
        {
            return audioClipsData[audioClipId].AudioClip;
        }

        private void SetAudioClipToSource(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }

        #endregion
    }
}

[System.Serializable]
public enum SoundType
{
    Click,
    Match,
    Placing,
    LevelSuccess,
}
