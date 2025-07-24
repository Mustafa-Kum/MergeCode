using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts._GameLogic.Logic.Controller
{
    public class QuestOrderController : MonoBehaviour
    {
        [SerializeField] private GridObjectProductDataContainer _productDataContainer;
        [SerializeField] private Button _questStartButton;
        [SerializeField] private Button _collectButton; 
        [SerializeField] private List<Quest> _quests;

        private Quest _activeQuest;
        private Quest _selectedQuest;

        private void OnEnable()
        {
            EventManager.UIEvents.OnQuestSelected += SelectQuest;

            if (_quests is { Count: > 0 })
            {
                DOVirtual.DelayedCall( 0.1f, () =>
                {
                    Quest quest = FindValidQuest();
                    quest?.OnClicked();
                }).SetLink(_quests[0].gameObject);
            }
            
            UpdateUIState();
        }

        private Quest FindValidQuest()
        {
            return _quests.Find(quest => quest.IsCompleted)
                   ?? _quests.Find(quest => quest.IsQuestActive())
                   ?? _quests[0];
        }

        private void OnDisable()
        {
            EventManager.UIEvents.OnQuestSelected -= SelectQuest;
        }

        private void Awake()
        {
            _questStartButton.onClick.AddListener(StartSelectedQuest);
            _collectButton.onClick.AddListener(CollectSelectedQuestReward);
            _collectButton.gameObject.SetActive(false);
            _collectButton.interactable = false;
        }

        private void Update()
        {
            _activeQuest?.UpdateQuest(Time.deltaTime);
        }

        private void SelectQuest(Quest quest)
        {
            _selectedQuest = quest;
            UpdateUIState();
        }

        private void StartSelectedQuest()
        {
            if (_activeQuest != null || _selectedQuest == null || !IsQuestAvailable(_selectedQuest)) return;

            _activeQuest = _selectedQuest;
            _activeQuest.OnQuestCompleted += OnQuestCompleted;
            _activeQuest.StartQuest();
            UpdateUIState();
        }

        private void OnQuestCompleted(Quest quest)
        {
            _collectButton.gameObject.SetActive(true);
            _collectButton.interactable = true;
            _questStartButton.gameObject.SetActive(false);
            _activeQuest = null;
            
            UpdateUIState();
        }

        private bool IsQuestAvailable(Quest quest)
        {
            int availableAmount = _productDataContainer.GetProductAmount(quest.ProductType);
            if (availableAmount >= quest.RequiredAmount)
            {
                _productDataContainer.RemoveProduct(quest.ProductType, quest.RequiredAmount);
                return true;
            }
            else
            {
                TDebug.Log("Insufficient product amount. Quest cannot be started.");
                return false;
            }
        }

        private void CollectSelectedQuestReward()
        {
            if (_selectedQuest == null || !_selectedQuest.IsCompleted) return;

            _selectedQuest.CollectReward();
            _collectButton.gameObject.SetActive(false);
            _questStartButton.gameObject.SetActive(true);

            _quests.Remove(_selectedQuest); 
            Destroy(_selectedQuest.gameObject); 
            _selectedQuest = null;

            EventManager.UIEvents.OnQuestRewardCollected?.Invoke();
            UpdateUIState(); 
        }

        private void UpdateUIState()
        {
            _questStartButton.interactable = _selectedQuest != null && _activeQuest == null;
            _collectButton.interactable = _selectedQuest != null && _selectedQuest.IsCompleted;
        }
    }
}
