using _Game.Scripts._GameLogic.Data.Product;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public class QuestRewardObjectGenerateProvider
    {
        private readonly CurrencyObjectDataContainer _currencyObjectDataContainer;
        private const float TweenDuration = 0.15f;

        public QuestRewardObjectGenerateProvider(CurrencyObjectDataContainer currencyObjectDataContainer)
        {
            _currencyObjectDataContainer = currencyObjectDataContainer;
        }

        public void GenerateQuestRewardObject(CurrencyType questRewardType)
        {
            var prefab = _currencyObjectDataContainer.GetCurrencyPrefab(questRewardType);
            var tile = RuntimeGridDataCache.FindEmptyRandomTile();
            if (tile == null) return;
            GenerateCurrencyObject(prefab, tile);
        }

        private void GenerateCurrencyObject(GridObject prefab, GridTile targetTile)
        {
            var animationSequence = DOTween.Sequence();

            var position = new Vector3(targetTile.transform.position.x, 0.8f, targetTile.transform.position.z);
            var currencyObject = Object.Instantiate(prefab, position, prefab.transform.rotation);

            Tween tween = currencyObject.transform.DOPunchScale(Vector3.one * 0.1f, TweenDuration, 1)
                .SetLink(currencyObject.gameObject);
            animationSequence.Append(tween);

            targetTile.SetTileObject(currencyObject);
            currencyObject.SetCurrentTile(targetTile);
        }
    }
}