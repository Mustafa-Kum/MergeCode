using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Controller;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GlobalObjectInitializer : MonoBehaviour
    {
        [SerializeField] private GridObjectDataContainer _gridObjectDataContainer;
        [SerializeField] private GridObjectProductDataContainer _gridObjectProductDataContainer;
        [SerializeField] private CurrencyObjectDataContainer _currencyObjectDataContainer;
        [SerializeField] private int _numberOfObjectsToSpawn;
        [SerializeField] private int _numberOfProductsToSpawn;
        private GridTile[,] _gridArray;

        private InitialGridObjectGenerateProvider _initialGenerateProvider;
        private MergeGridObjectGenerateProvider _mergeGenerateProvider;
        private ProductGridObjectGenerateProvider _generateProvider;
        private QuestRewardObjectGenerateProvider _questRewardObjectGenerateProvider;

        private void Awake()
        {
            _initialGenerateProvider = new InitialGridObjectGenerateProvider(_gridObjectDataContainer,_numberOfObjectsToSpawn);
            _mergeGenerateProvider = new MergeGridObjectGenerateProvider(_gridObjectDataContainer);
            _generateProvider = new ProductGridObjectGenerateProvider(_gridObjectProductDataContainer, _currencyObjectDataContainer,_numberOfProductsToSpawn);
            _questRewardObjectGenerateProvider = new QuestRewardObjectGenerateProvider(_currencyObjectDataContainer);
        }

        private void Start() => InitializeGridObjects();

        private void OnEnable()
        {
            EventManager.GridEvents.OnGridObjectMerged += OnGridObjectMerged;
            EventManager.GridEvents.OnProductProduced += ProduceProduct;
            EventManager.GridEvents.OnProductCurrencyReward += ProduceCurrency;
            EventManager.UIEvents.OnGridObjectGenerateUIClicked += GenerateRandomSingleObject;
            EventManager.UIEvents.OnCurrencyRewardCollected += GenerateQuestRewardObject;
            EventManager.DebugEvents.OnDebugGenerateButtonClicked += GenerateRandomSingleObject;
        }

        private void OnDisable()
        {
            EventManager.GridEvents.OnGridObjectMerged -= OnGridObjectMerged;
            EventManager.GridEvents.OnProductProduced -= ProduceProduct;
            EventManager.GridEvents.OnProductCurrencyReward -= ProduceCurrency;
            EventManager.UIEvents.OnGridObjectGenerateUIClicked -= GenerateRandomSingleObject;
            EventManager.UIEvents.OnCurrencyRewardCollected -= GenerateQuestRewardObject;
            EventManager.DebugEvents.OnDebugGenerateButtonClicked -= GenerateRandomSingleObject;
        }

        [Button("Spawn Grid Objects", ButtonSizes.Large)]
        [GUIColor(0.8f, 0.8f, 1)]
        private void InitializeGridObjects()
        {
            _gridArray = RuntimeGridDataCache.GetGridArray();
            if (_gridArray == null)
            {
                TDebug.LogError("Grid array is not set in RuntimeGridDataCache.");
                return;
            }

            _initialGenerateProvider.SpawnInitialObjects(_gridArray, transform);
        }
        
        [Button] 
        private void SpawnDesiredTypeOfObjects(GridObject.GridObjectData data)
        {
            _gridArray = RuntimeGridDataCache.GetGridArray();
            if (_gridArray == null)
            {
                TDebug.LogError("Grid array is not set in RuntimeGridDataCache.");
                return;
            }

            _initialGenerateProvider.SpawnDesiredTypeOfObjects(_gridArray, transform, data);
        }
        
        [Button]
        private void GenerateRandomSingleObject()
        {
            _gridArray = RuntimeGridDataCache.GetGridArray();
            if (_gridArray == null)
            {
                TDebug.LogError("Grid array is not set in RuntimeGridDataCache.");
                return;
            }

            _initialGenerateProvider.SpawnRandomSingleObject(_gridArray, transform);
        }
        
        private void OnGridObjectMerged(GridObject.GridObjectData data, Vector2Int position, int mergeableCount)
        {
            _mergeGenerateProvider.SpawnMergedObjects(data, position, mergeableCount, transform);
        }
        
        private void ProduceProduct(GridTile source ,GridObjectProductDataContainer.ProductType productType)
        {
            _generateProvider.GenerateProductObject(source ,productType);
        }
        
        private void GenerateQuestRewardObject(List<Quest.QuestReward> questRewards)
        {
            foreach (var questReward in questRewards)
            {
                _questRewardObjectGenerateProvider.GenerateQuestRewardObject(questReward.Type);
            }
        }
        
        private void ProduceCurrency(GridTile source)
        {
            _generateProvider.GenerateCurrencyObject(source);
        }
    }
}