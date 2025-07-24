using Abu;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Tutorial.Logic
{
    public class TutorialFadeAnimationOnStep : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public TutorialFadeImage tutorialFadeImage;

        #endregion

        #region PUBLIC METHODS

        public void FadeIn(float duration)
        {
            tutorialFadeImage.InterpolateOnceSmoothness(0.03f, duration);
        }
        
        public void DelayedFadeIn(float delay)
        {
            DOVirtual.DelayedCall(delay, () => FadeIn(0.25f));
        }
        
        public void FadeOut(float duration)
        {
            tutorialFadeImage.InterpolateOnceSmoothness(1f, duration);
        }

        #endregion
    }
}