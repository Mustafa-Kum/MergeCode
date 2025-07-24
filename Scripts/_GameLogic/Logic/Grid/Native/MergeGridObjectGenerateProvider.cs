using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts.Helper.Extensions.System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public class MergeGridObjectGenerateProvider : BaseObjectGenerateService
    {
        public MergeGridObjectGenerateProvider(GridObjectDataContainer container) : base(container) { }

        public void SpawnMergedObjects(GridObject.GridObjectData sourceData, Vector2Int mergeCenter, int mergeCount, Transform parentTransform)
        {
            GridObject nextLevelPrefab = ObjectDataContainer.GetNextLevelGridObject(sourceData);
            GridObject currentLevelPrefab = ObjectDataContainer.GetCurrentLevelGridObject(sourceData);

            if (nextLevelPrefab == null)
            {
                TDebug.LogWarning("Upgrade path not found for current object.");
                return;
            }

            List<GridTile> availableTiles = RuntimeGridDataCache.FindEmptyNeighborTiles(mergeCenter, mergeCount);
            var animationSequence = DOTween.Sequence();

            if (mergeCount >= 9 && mergeCount % 3 == 0)
            {
                int higherLevelCount = mergeCount / 9; 
                GridObject higherLevelPrefab = ObjectDataContainer.GetNextLevelGridObject(nextLevelPrefab.GetGridObjectData());

                if (higherLevelPrefab != null)
                {
                    SpawnObjects(higherLevelPrefab, higherLevelCount, availableTiles, animationSequence, parentTransform);
                }

                int remainingCount = mergeCount % 9;
                if (remainingCount > 0)
                {
                    ProcessRemainingObjects(remainingCount, nextLevelPrefab, currentLevelPrefab, availableTiles, animationSequence, parentTransform);
                }
            }
            else
            {
                ProcessRemainingObjects(mergeCount, nextLevelPrefab, currentLevelPrefab, availableTiles, animationSequence, parentTransform);
            }
        }

        private void ProcessRemainingObjects(int count, GridObject nextLevelPrefab, GridObject currentLevelPrefab, List<GridTile> availableTiles, Sequence animationSequence, Transform parentTransform)
        {
            int nextLevelCount = count / 3;
            int currentLevelCount = count % 3;

            if (nextLevelCount > 0)
            {
                SpawnObjects(nextLevelPrefab, nextLevelCount, availableTiles, animationSequence, parentTransform);
            }

            if (currentLevelCount > 0)
            {
                SpawnObjects(currentLevelPrefab, currentLevelCount, availableTiles, animationSequence, parentTransform);
            }
        }

        private void SpawnObjects(GridObject prefab, int count, List<GridTile> availableTiles, Sequence animationSequence, Transform parentTransform)
        {
            for (int i = 0; i < count && i < availableTiles.Count; i++)
            {
                SpawnSingleObject(prefab, availableTiles[i], animationSequence, parentTransform, false, true);
            }
            availableTiles.RemoveRange(0, Mathf.Min(count, availableTiles.Count));
        }
    }
}
