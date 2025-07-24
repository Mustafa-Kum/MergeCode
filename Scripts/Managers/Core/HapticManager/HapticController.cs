using System;
using System.Collections;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.HapticManager 
{
    public class HapticController : MonoBehaviour
    {
        public float minimumDelayBetweenHaptics = 0.2f;
        private bool _isHapticPlaying = false;
        private Queue<HapticPatterns.PresetType> _hapticQueue;

        [SerializeField] private bool enableDebugging = false;

        private void Awake() => _hapticQueue = new Queue<HapticPatterns.PresetType>();

        [Button]
        public void QueueHaptic(HapticPatterns.PresetType preset)
        {
            if (!_isHapticPlaying)
            {
                _isHapticPlaying = true;
                _hapticQueue.Enqueue(preset);
                StartCoroutine(ProcessHapticQueue());
            }
        }
        
        [Button]
        public void QueueHapticCount(HapticPatterns.PresetType preset, int count)
        {
            if (count <= 0)
            {
                Debug.LogWarning("Haptic count must be greater than 0. Ignoring request.");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                _hapticQueue.Enqueue(preset);
            }

            if (!_isHapticPlaying)
            {
                StartCoroutine(ProcessHapticQueue());
            }
        }


        private IEnumerator ProcessHapticQueue()
        {
            while (_hapticQueue.Count > 0)
            {
                HapticPatterns.PresetType currentPreset = _hapticQueue.Dequeue();
                HapticPatterns.PlayPreset(currentPreset);

                if (enableDebugging)
                {
                    Debug.Log("Haptic played: " + currentPreset);
                }

                yield return new WaitForSeconds(minimumDelayBetweenHaptics);
            }

            _isHapticPlaying = false; // Döngü dışındaki 'continue' kullanılmıyor
            _hapticQueue.Clear();
        }
    }
}