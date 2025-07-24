using System;
using _Game.Scripts.Managers.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI.Buttons
{
    public class LevelStartButton : ButtonBase
    {
        [SerializeField] private Image _blackBG;

        private Sequence _blackBgSequence;

        private void OnEnable()
        {
            _blackBG.enabled = false;
        }

        protected override void OnClicked()
        {
            RectTransform blackBgRectTransform = _blackBG.rectTransform; 
            _blackBgSequence = DOTween.Sequence();
            
            _blackBG.enabled = true;
            _blackBgSequence.Append(blackBgRectTransform.DOSizeDelta(new Vector2(3000, 3000), 0.5f)).SetEase(Ease.Linear);
 
            EventDelay();
        }

        private void EventDelay()
        {
            _coroutineService.StartDelayedRoutine(EventMethod, 0.8f);
        }

        private void EventMethod()
        {
            EventManager.InGameEvents.LevelStart?.Invoke();
            EventManager.TutorialEvents.LevelStartTutorialCaching?.Invoke(1);
        }
        
        public void OnClickButton()
        {
            OnClicked();
        }
    }   
}