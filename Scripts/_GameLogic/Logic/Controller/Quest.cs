using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts.Managers.Core;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class Quest : MonoBehaviour
    {
        public GridObjectProductDataContainer.ProductType ProductType;
        public GridObjectProductDataContainer ProductDataContainer;
        public int RequiredAmount; 
        public float TimeLimit; 

        private DateTime _startTime; 
        private bool _isCompleted;
        private bool _isActive;

        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _requiredAmountText;
        private Button _button;
        public event Action<Quest> OnQuestCompleted;

        public List<QuestReward> QuestRewards;
        public bool IsCompleted => _isCompleted;
        
        [Serializable]
        public class QuestReward
        {
            public CurrencyType Type;
            public int Amount;
        }

        private void OnEnable()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => EventManager.UIEvents.OnQuestSelected?.Invoke(this));
            UpdateRequiredAmountUI();
            
            if (_isActive)
            {
                UpdateQuest(0);
            }
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void StartQuest()
        {
            _startTime = DateTime.Now; 
            _isCompleted = false;
            _isActive = true;
            UpdateRequiredAmountUI();
        }

        public void UpdateQuest(float deltaTime)
        {
            if (!_isActive) return;

            TimeSpan elapsedTime = DateTime.Now - _startTime;
            float remainingTime = TimeLimit - (float)elapsedTime.TotalSeconds;
            
            if (remainingTime <= 0)
            {
                CompleteQuest();
            }
            else
            {
                UpdateTimerUI(remainingTime);
            }
        }
        
        public bool IsQuestActive()
        {
            return _isActive;
        }

        private void CompleteQuest()
        {
            _isCompleted = true;
            _isActive = false;
            OnQuestCompleted?.Invoke(this); 
        }

        public void CollectReward()
        {
            if (!_isCompleted) return;
            EventManager.UIEvents.OnCurrencyRewardCollected?.Invoke(QuestRewards);
            gameObject.SetActive(false);
        }
        
        private void UpdateTimerUI(float remainingTime = -1)
        {
            if (remainingTime < 0)
            {
                remainingTime = TimeLimit - (float)(DateTime.Now - _startTime).TotalSeconds;
            }
            
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);

            _timerText.text = $"{minutes:00}:{seconds:00}";
        }

        private void UpdateRequiredAmountUI()
        {
            int currentAmount = ProductDataContainer.GetProductAmount(ProductType);
            _requiredAmountText.text = $"{currentAmount}/{RequiredAmount}";
        }

        public void OnClicked()
        {
            _button.onClick.Invoke();
        }
    }
}
