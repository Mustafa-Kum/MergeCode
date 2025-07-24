using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts.Helper.Extensions.System;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public class InitialGridObjectGenerateProvider : BaseObjectGenerateService
    {
        private readonly int _numberOfObjectsToSpawn;
        private readonly Random _random;

        public InitialGridObjectGenerateProvider(GridObjectDataContainer container, int numberOfObjectsToSpawn) : base(container)
        {
            _numberOfObjectsToSpawn = numberOfObjectsToSpawn;
            _random = new Random();
        }

        public void SpawnInitialObjects(GridTile[,] gridArray, Transform parent)
        {
            var availablePositions = RuntimeGridDataCache.GetAvailablePositions(gridArray);
            if (availablePositions.Count < _numberOfObjectsToSpawn)
            {
                TDebug.LogWarning(
                    $"Not enough available positions ({availablePositions.Count}) to spawn all objects ({_numberOfObjectsToSpawn}).");
                return;
            }

            var animationSequence = DOTween.Sequence();

            for (var i = 0; i < _numberOfObjectsToSpawn; i++)
            {
                var randomPosition = GetRandomPosition(availablePositions);
                var tile = RuntimeGridDataCache.GetTileAtPosition(randomPosition);

                if (tile == null || !tile.IsEmpty) continue;

                var prefab = ObjectDataContainer.GetBalancedRandomTypeFirstLevelGridObject();
                SpawnSingleObject(prefab, tile, animationSequence, parent, false, false);
            }
        }

        public void SpawnDesiredTypeOfObjects(GridTile[,] gridArray, Transform parent, GridObject.GridObjectData data)
        {
            var availablePositions = RuntimeGridDataCache.GetAvailablePositions(gridArray);
            if (availablePositions.Count < 1)
            {
                TDebug.LogWarning($"Not enough available positions ({availablePositions.Count}) to spawn the object.");
                return;
            }

            var animationSequence = DOTween.Sequence();

            var randomPosition = GetRandomPosition(availablePositions);
            var tile = RuntimeGridDataCache.GetTileAtPosition(randomPosition);

            if (tile == null || !tile.IsEmpty) return;

            var prefab = ObjectDataContainer.GetCurrentLevelGridObject(data);
            SpawnSingleObject(prefab, tile, animationSequence, parent, false, true);
        }
        
        

        private Vector2Int GetRandomPosition(List<Vector2Int> availablePositions)
        {
            var randomIndex = _random.Next(0, availablePositions.Count);
            var position = availablePositions[randomIndex];
            availablePositions[randomIndex] = availablePositions[^1];
            availablePositions.RemoveAt(availablePositions.Count - 1);
            return position;
        }

        public void SpawnRandomSingleObject(GridTile[,] gridArray, Transform transform)
        {
            var availablePositions = RuntimeGridDataCache.GetAvailablePositions(gridArray);
            if (availablePositions.Count < 1)
            {
                TDebug.LogWarning($"Not enough available positions ({availablePositions.Count}) to spawn the object.");
                return;
            }

            var animationSequence = DOTween.Sequence();

            var randomPosition = GetRandomPosition(availablePositions);
            var tile = RuntimeGridDataCache.GetTileAtPosition(randomPosition);

            if (tile == null || !tile.IsEmpty) return;

            var prefab = ObjectDataContainer.GetBalancedRandomTypeFirstLevelGridObject();
            SpawnSingleObject(prefab, tile, animationSequence, transform, false, true);
        }
    }
}