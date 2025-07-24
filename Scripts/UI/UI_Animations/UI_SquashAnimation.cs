using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_SquashAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform boxRectTransform;
    [SerializeField] private RectTransform circleRectTransform;
    
    [Header("Box Animation Settings")]
    [SerializeField] private float boxBounceHeight = 50f;
    [SerializeField] private float boxBounceDuration = 0.5f;
    [SerializeField] private float boxSquashStretchFactor = 0.2f;

    [Header("Circle Animation Settings")]
    [SerializeField] private float circleSquashFactor = 0.3f;
    [SerializeField] private float circleSquashDuration = 0.5f;
    [SerializeField] private float circleDelayBetweenLoops = 1f;
    [SerializeField] private Color _startColor;

    private void Start()
    {
        PlayCircleSquashAnimationInLoop();
    }

    private void PlayBoxBounceAnimation()
    {
        Sequence boxBounceSequence = CreateBoxBounceSequence(boxRectTransform, boxBounceHeight, boxBounceDuration, boxSquashStretchFactor);
        boxBounceSequence.Restart();
        boxBounceSequence.Play();
    }

    private void PlayCircleSquashAnimationInLoop()
    {
        Sequence circleSquashSequence = CreateCircleSquashSequence(circleRectTransform, circleSquashFactor, circleSquashDuration);
        circleSquashSequence.SetLoops(-1, LoopType.Restart)
                            .AppendInterval(circleDelayBetweenLoops);
    }

    private Sequence CreateBoxBounceSequence(RectTransform target, float height, float duration, float squashStretchFactor)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(target.DOAnchorPosY(height, duration)
            .SetEase(Ease.OutQuad)
            .OnUpdate(() => UpdateBoxScale(target, height, squashStretchFactor))
            .OnComplete(() => ResetBoxScale(target))
        );

        sequence.Append(target.DOAnchorPosY(15, 0.7f)
            .SetEase(Ease.InOutBack).OnComplete((() =>
            {
                circleRectTransform.GetComponent<Image>().DOColor(_startColor, 0.1f);
            }))
        );

        sequence.Join(CreateCircleSquashSequenceOnes(circleRectTransform, circleSquashFactor, circleSquashDuration).SetDelay(0.3f));

        sequence.Append(target.DOScale(Vector3.one, duration * 0.5f)
            .SetEase(Ease.OutElastic)
        );

        return sequence;
    }

    private Sequence CreateCircleSquashSequence(RectTransform target, float squashFactor, float duration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(target.DOScale(new Vector3(1 + squashFactor, 1 - squashFactor, 1), duration)
            .SetEase(Ease.OutQuad)
        );

        sequence.Append(target.DOScale(Vector3.one, duration)
            .SetEase(Ease.InQuad).OnComplete(() =>
            {
                circleRectTransform.GetComponent<Image>().color = Color.white;
                
                PlayBoxBounceAnimation();
            })
        );

        return sequence;
    }
    
    private Sequence CreateCircleSquashSequenceOnes(RectTransform target, float squashFactor, float duration)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(target.DOScale(new Vector3(1 + squashFactor, 1 - squashFactor, 1), duration)
            .SetEase(Ease.OutQuad)
        );

        sequence.Append(target.DOScale(Vector3.one, duration)
            .SetEase(Ease.InQuad));

        return sequence;
    }

    private void UpdateBoxScale(RectTransform target, float height, float squashStretchFactor)
    {
        float progress = CalculateProgress(target.anchoredPosition.y, height);
        float scale = CalculateSquashStretchScale(progress, squashStretchFactor);
        ApplyScale(target, scale);
    }

    private float CalculateProgress(float currentPosition, float maxPosition)
    {
        return currentPosition / maxPosition;
    }

    private float CalculateSquashStretchScale(float progress, float squashStretchFactor)
    {
        return 1 + (squashStretchFactor * progress);
    }

    private void ApplyScale(RectTransform target, float scale)
    {
        target.localScale = new Vector3(1 / scale, scale, 1);
    }

    private void ResetBoxScale(RectTransform target)
    {
        target.DOScale(Vector3.one, 0.5f);
    }
}
