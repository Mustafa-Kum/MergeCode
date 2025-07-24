using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class PullingBackAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _tableTransform;
        [SerializeField] private Transform _royalBoyTransform;
        [SerializeField] private GameObject _allParent;

        private Sequence _sequence;

        private void OnDisable()
        {
            _allParent.GetComponent<Image>().DOFade(0.99f, 0);
        }

        public void StartAnimation()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(_tableTransform.DOMoveY(_tableTransform.position.y - 75f, 0.2f));
            _sequence.Join(_royalBoyTransform.DOMoveY(_royalBoyTransform.position.y + 75f, 0.2f));
            
            _sequence.Append(_tableTransform.DOLocalMoveY(2050f, 0.2f).OnComplete(() =>
            {
                _allParent.GetComponent<Image>().DOFade(0, 0.2f);
                
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    _allParent.SetActive(false);
                });
            }));
            
            _sequence.Join(_royalBoyTransform.DOMoveY(-2050, 0.2f));
        }
    }
}