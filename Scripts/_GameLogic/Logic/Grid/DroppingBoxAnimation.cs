using System;
using _Game.Scripts._GameLogic.Logic.Grid.Native;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class DroppingBoxAnimation : MonoBehaviour, IGridObjectClickableAction
    {
        [SerializeField] private GameObject _boxShadowSprite; // Default Scale 0f
        [SerializeField] private GameObject _boxSprite; // Default color Alpha 0f
        [SerializeField] private GameObject _gridObjectShadowSprite;
        [SerializeField] private ParticleSystem _boxDroppingParticle;
        
        private Sequence _boxDroppingSequence;
        private SpriteRenderer _boxSpriteRenderer;
        private SpriteRenderer _gridObjectSpriteRenderer;
        private SpriteRenderer _gridObjectShadowSpriteRenderer;

        private void Awake()
        {
            if (_boxSprite != null)
                _boxSpriteRenderer = _boxSprite.GetComponent<SpriteRenderer>();
            
            if (_boxShadowSprite != null)
                _gridObjectSpriteRenderer = transform.GetComponent<SpriteRenderer>();
            
            if (_gridObjectShadowSprite != null)
                _gridObjectShadowSpriteRenderer = _gridObjectShadowSprite.GetComponent<SpriteRenderer>();
        }
        
        public void OnObjectGenerated(GridObject gridObject)
        {
            BoxFallingAnimationSequence();
        }

        public void OnObjectClick(GridObject gridObject)
        {
        }

        public void OnObjectMerge(GridObject gridObject)
        {
        }
        
        private void BoxFallingAnimationSequence()
        {
            _boxDroppingSequence = DOTween.Sequence();
            
            if (_boxShadowSprite == null && _boxSprite == null)
                return;

            _gridObjectSpriteRenderer.color = new Color(255, 255, 255, 0);
            _gridObjectShadowSpriteRenderer.color = new Color(255, 255, 255, 0);
            
            _boxDroppingSequence.Append(_boxShadowSprite.transform.DOScale(new Vector3(8f, 8f, 8f), 0.5f)).SetLink(_boxShadowSprite);
            _boxDroppingSequence.Join(_boxSprite.transform.DOLocalMoveY(1.5f, 0.3f).SetDelay(0.2f).SetLink(_boxSprite));
            
            _boxDroppingSequence.Join(_boxSpriteRenderer.DOFade(1f, 0.3f).SetDelay(0.15f).SetLink(_boxSpriteRenderer.gameObject)
                .OnComplete(() =>
                {
                    _boxSpriteRenderer.DOFade(0f, 0.2f).SetLink(_boxSpriteRenderer.gameObject);
                    _boxShadowSprite.SetActive(false);
                }));
            
            _boxDroppingSequence.Join(_gridObjectSpriteRenderer.DOFade(1f, 0.1f).SetDelay(0.15f).SetLink(_gridObjectSpriteRenderer.gameObject));
            _boxDroppingSequence.Join(_gridObjectShadowSpriteRenderer.DOFade(1f, 0.1f).SetDelay(0.15f).SetLink(_gridObjectShadowSpriteRenderer.gameObject));
            
            _boxDroppingSequence.InsertCallback(_boxDroppingSequence.Duration() - 0.4f, () =>
            {
                if (_boxDroppingParticle != null)
                {
                    _boxDroppingParticle.Play();
                }
            }).SetLink(_boxDroppingParticle.gameObject);
        }
    }
}