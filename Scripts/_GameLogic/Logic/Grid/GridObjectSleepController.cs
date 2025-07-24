using System;
using _Game.Scripts.Managers.Core;
using TMPro;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridObjectSleepController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _sleepTimeText;
        
        [SerializeField] private int _sleepTime;
        private float _sleepTimeRemaining;
        private bool IsSleeping { get; set; }

        private void Awake()
        {
            _sleepTimeRemaining = _sleepTime;
            
            Sleep();
        }
        
        private void Update()
        {
            if (!IsSleeping) return;
            UpdateTimer();
            if (_sleepTimeRemaining <= 0)
            {
                WakeUp();
            }
        }

        private void UpdateTimer()
        {
            _sleepTimeRemaining -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_sleepTimeRemaining);
            _sleepTimeText.text = timeSpan.ToString("mm\\:ss");
        }
        
        private void Sleep()
        {
            IsSleeping = true;
            EventManager.TimerEvents.UIOnTimerBegan?.Invoke(this, IsSleeping);
        }
        
        private void WakeUp()
        {
            IsSleeping = false;
            EventManager.TimerEvents.UIOnTimerEnded?.Invoke(this, IsSleeping);
        }
    }
}