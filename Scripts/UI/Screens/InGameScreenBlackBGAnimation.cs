using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class InGameScreenBlackBgAnimation : MonoBehaviour
    {
        [SerializeField] private Image _blackBG;

        private Sequence _blackBgSequence;

        private void OnEnable()
        {
            // RectTransform blackBgRectTransform = _blackBG.GetComponent<RectTransform>();
            //
            // _blackBgSequence = DOTween.Sequence();
            //
            // _blackBgSequence.Append(blackBgRectTransform.DOSizeDelta(Vector2.zero, 0.8f)).SetEase(Ease.Linear);
        }
    }
}