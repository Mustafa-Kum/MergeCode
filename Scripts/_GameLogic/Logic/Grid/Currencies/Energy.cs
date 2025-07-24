using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Currencies
{
    public sealed class Energy : GridObject, IObjectClickAction
    {
        private List<ICurrencyClickableComponent> CurrencyClickableComponents { get; set; }
        private bool IsClickable { get; set; } = true;
        
        private void Start()
        {
            CurrencyClickableComponents = new List<ICurrencyClickableComponent>();
            GetComponentsInChildren(CurrencyClickableComponents);
        }
        
        public void Execute()
        {
            if (!IsClickable) return;
            
            EventManager.ProductEvents.ProductCurrencyRewardCollect?.Invoke(CurrencyType.Energy);
            
            EventManager.Resource.CurrencyCollected?.Invoke(CurrencyType.Energy, transform.position);
            
            CurrencyClickableComponents.ForEach(x => x.Execute());
            
            SelfDestruct();
            
            IsClickable = false;
        }
        
        private void SelfDestruct()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 1, 0.5f)).SetLink(gameObject);
            sequence.Append(transform.DOScale(Vector3.zero, 0.25f)).SetLink(gameObject);
            sequence.AppendCallback(() => Destroy(gameObject));
        }
    }
}