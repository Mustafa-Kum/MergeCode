using System.Collections.Generic;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace _Game.Scripts._GameLogic.Logic.Grid.Currencies
{
    public sealed class Product : GridObject, IObjectClickAction
    {
        private List<IProductClickableComponent> ProductClickableComponents { get; set; }
        private bool IsClickable { get; set; } = true;
        
        private void Start()
        {
            ProductClickableComponents = new List<IProductClickableComponent>();
            GetComponentsInChildren(ProductClickableComponents);
        }

        public void Execute()
        {
            if (!IsClickable) return;
            ProductClickableComponents.ForEach(x => x.Execute());
            
            EventManager.ProductEvents.OnProductCollect?.Invoke(ProductData.ProductType, transform.position);
            
            IsClickable = false;
        }
        
        public void SelfDestruct()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 1, 0.5f)).SetLink(gameObject);
            sequence.Append(transform.DOScale(Vector3.zero, 0.25f)).SetLink(gameObject);
            sequence.AppendCallback(() => Destroy(gameObject));
        }
        
        public Vector3 GetPosition() => transform.position;
    }
}