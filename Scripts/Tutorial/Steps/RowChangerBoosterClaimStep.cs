using _Game.Scripts.Tutorial.Logic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Tutorial.Steps
{
    public class RowChangerBoosterClaimStep : BaseTutorialStep
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private RectTransform targetRectTransform;
        [SerializeField] private Sprite spriteToSpawn;
        [SerializeField] private RectTransform spawnPointRectTransform;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float moveDuration = 1f;
        [SerializeField] private float spawnDelay = 0.2f;

        #endregion

        #region INHERITED METHODS

        public override void ActivateStep()
        {
            base.ActivateStep();
        }

        public override void DisableStep()
        {
            base.DisableStep();
        }

        #endregion

        #region PUBLIC METHODS

        public void SpawnMultipleSprites(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float delay = i * spawnDelay;
                SpawnAndMoveSprite(delay);
            }
        }

        #endregion
        
        #region PRIVATE METHODS

        private GameObject CreateSpriteObject()
        {
            GameObject spriteObj = new GameObject("SpawnedSprite", typeof(Image));
            spriteObj.transform.SetParent(canvas.transform, false);
            spriteObj.GetComponent<RectTransform>().anchoredPosition = spawnPointRectTransform.anchoredPosition;
            return spriteObj;
        }

        private void SetSprite(GameObject spriteObject, Sprite sprite)
        {
            Image imageComponent = spriteObject.GetComponent<Image>();
            imageComponent.sprite = sprite;
        }

        private void AnimateSpriteToTarget(RectTransform spriteRectTransform, float delay)
        {
            Sequence animationSequence = DOTween.Sequence();
            animationSequence.AppendInterval(delay)
                             .Append(spriteRectTransform.DOMove(targetRectTransform.position, moveDuration))
                             .OnComplete(() => GameObject.Destroy(spriteRectTransform.gameObject));
        }

        private void SpawnAndMoveSprite(float delay)
        {
            GameObject spawnedSpriteObj = CreateSpriteObject();
            SetSprite(spawnedSpriteObj, spriteToSpawn);
            AnimateSpriteToTarget(spawnedSpriteObj.GetComponent<RectTransform>(), delay);
        }

        #endregion
    }
}