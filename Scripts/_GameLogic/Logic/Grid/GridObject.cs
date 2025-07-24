using System;
using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts._GameLogic.Data.Product;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.Managers.Core;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class GridObject : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private GridTile _currentTile;
        [SerializeField] private GridObjectData _gridObjectData;
        [SerializeField] protected GridObjectProductData ProductData;
        [SerializeField] private GridObjectSleepController _gridObjectSleepController;
        [SerializeField] private GameObject _gridObjectOutline;
        private List<IGridObjectClickableAction> _gridObjectClickableActions = new();
        private bool HasProduct => _gridObjectData.Level >= 4 && ProductData.ProductRenderer != null;
        private bool HasSleep => _gridObjectData.Level >= 4 && ProductData.SleepTimerRenderer != null;
        private bool IsSleeping { get; set; }
        private bool _isSleepToggleActive = false;
        private bool HasOutline => _gridObjectOutline != null;
        private GameObject GridObjectOutline => _gridObjectOutline;

        private readonly Queue<Action> _productQueue = new();
        public UnityAction OnProductCheck;

        [Serializable]
        public sealed class GridObjectData
        {
            public int Level;
            public GridObjectDataContainer.GridObjectType Type;
        }

        [Serializable]
        [CanBeNull]
        public sealed class GridObjectProductData
        {
            public GridObjectProductDataContainer.ProductType ProductType;
            public SpriteRenderer ProductRenderer;
            public SpriteRenderer CurrencyRenderer;
            public GameObject SleepTimerRenderer;
            public GameObject RegeneratePopup;
        }
        
        private void Awake()
        {
            _gridObjectClickableActions = new List<IGridObjectClickableAction>(GetComponentsInChildren<IGridObjectClickableAction>());
            RegisteringActionsInQueue();
            EnableGridObjectOutline(false);
        }

        private void OnEnable()
        {
            EventManager.TimerEvents.UIOnTimerBegan += OnGridObjectSleepBegan;
            EventManager.TimerEvents.UIOnTimerEnded += OnGridObjectSleepEnded;
            
            EnableProductFrame(ProductData.ProductRenderer);
            OnProductCheck += TryExecuteProductOrder;
            OnProductCheck += ToggleSleepTimerState;
        }

        private void OnDisable()
        {
            EventManager.TimerEvents.UIOnTimerBegan -= OnGridObjectSleepBegan;
            EventManager.TimerEvents.UIOnTimerEnded -= OnGridObjectSleepEnded;
        }

        public void SetCurrentTile(GridTile tile) => _currentTile = tile;

        public GridObjectData GetGridObjectData() => _gridObjectData;

        public GridObjectProductData GetGridObjectProductData() => ProductData;
        
        public List<IGridObjectClickableAction> GetGridObjectClickableActions() => _gridObjectClickableActions;

        private void RegisteringActionsInQueue()
        {
            if(!HasProduct) return;
            _productQueue.Enqueue(ProductProduceOrder);
            if (HasSleep)
            {
                _productQueue.Enqueue(ProductSleepOrder);
                _productQueue.Enqueue(SleepProductProduceOrder);
            }
            
            _productQueue.Enqueue(ProductCurrencyRewardOrder);
        }
        
        public void EnableGridObjectOutline(bool condition)
        {
            if (!HasOutline) return;
            GridObjectOutline.SetActive(condition);
        }

        private void OnGridObjectSleepBegan(GridObjectSleepController controller, bool isSleeping)
        {
            if (!HasSleep) return;
            if (controller != _gridObjectSleepController) return;
            IsSleeping = isSleeping;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void OnGridObjectSleepEnded(GridObjectSleepController controller, bool isSleeping)
        {
            if (!HasSleep) return;
            if (controller != _gridObjectSleepController) return;
            IsSleeping = isSleeping;
            TryExecuteProductOrder();
        }
        
        private void DisableProductFrame(SpriteRenderer spriteRenderer)
        {
            if (!HasProduct) return;
            spriteRenderer.gameObject.SetActive(false);
        }
        
        private void EnableProductFrame(SpriteRenderer spriteRenderer)
        {
            if (!HasProduct) return;
            spriteRenderer.gameObject.SetActive(true);
        }
        
        private void TryExecuteProductOrder()
        {
            if (_productQueue.Count <= 0 || !HasProduct || IsSleeping) return;
            Action order = _productQueue.Dequeue();
            order?.Invoke(); 
    
            TDebug.Log("Product Order Sent. Remaining Orders: " + _productQueue.Count);

            if (_productQueue.Count > 0) return;
            _productQueue.Clear();
            Destroy(gameObject);
        }
        
        private void ToggleSleepTimerState()
        {
            if (!HasSleep && !IsSleeping) return;

            _isSleepToggleActive = !_isSleepToggleActive;
            ProductData.RegeneratePopup.SetActive(_isSleepToggleActive);
        }
        
        private void ProductProduceOrder()
        {
            if (!HasProduct) return;
            
            TDebug.Log("Product Produce Begin");
            EventManager.GridEvents.OnProductProduced?.Invoke(_currentTile, ProductData.ProductType);
            
            DisableProductFrame(ProductData.ProductRenderer);

            if (HasSleep)
            {
                ProductData.SleepTimerRenderer.SetActive(true);
            }
            else
            {
                EnableProductFrame(ProductData.CurrencyRenderer);
            }
        }
        
        private void ProductSleepOrder()
        {
            if (!HasProduct) return;
            if (!HasSleep) return;
            TDebug.Log("Product Sleep Begin");
            ProductData.SleepTimerRenderer.SetActive(false);
            ProductData.ProductRenderer.gameObject.SetActive(true);
        }
        
        
        private void SleepProductProduceOrder()
        {
            if (!HasProduct) return;
            if (!HasSleep) return;
            TDebug.Log("Product Sleep Produce Begin");
            EventManager.GridEvents.OnProductProduced?.Invoke(_currentTile, ProductData.ProductType);
            DisableProductFrame(ProductData.ProductRenderer);
            EnableProductFrame(ProductData.CurrencyRenderer);
        }
        
        private void ProductCurrencyRewardOrder()
        {
            if (!HasProduct) return;
            TDebug.Log("Product Currency Reward Begin");
            EventManager.GridEvents.OnProductCurrencyReward?.Invoke(_currentTile);
            DisableProductFrame(ProductData.CurrencyRenderer);
        }
    }
}
