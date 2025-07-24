using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Grid;
using _Game.Scripts.Helper.Extensions.System;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public abstract class BaseObjectGenerateService
    {
        protected readonly GridObjectDataContainer ObjectDataContainer;
        private readonly Dictionary<GridObjectDataContainer.GridObjectType, Vector3> _spawnOffsetDictionary 
            = new()
            {
                { GridObjectDataContainer.GridObjectType.Brick, new Vector3(-0.5f, 1.1f, -0.75f) },
                { GridObjectDataContainer.GridObjectType.Wood, new Vector3(-1f, 1.1f, -1.25f) },
                { GridObjectDataContainer.GridObjectType.Cow, new Vector3(0f, 1.1f, 0f) },
                { GridObjectDataContainer.GridObjectType.Chicken, new Vector3(0f, 1.1f, 0f) },
                { GridObjectDataContainer.GridObjectType.Crop, new Vector3(0f, 1.1f, 0f) },

            };
        
        protected BaseObjectGenerateService(GridObjectDataContainer container)
        {
            ObjectDataContainer = container;
        }

        protected void SpawnSingleObject(GridObject prefab, GridTile tile, Sequence animationSequence, Transform parentTransform, bool logSpawn, bool sendInterfaceEvent)
        {
            Vector3 spawnPosition = CalculateSpawnPosition(prefab.GetGridObjectData().Type, tile);
            GridObject newObject = Object.Instantiate(prefab, spawnPosition, prefab.transform.rotation, parentTransform);
            
            animationSequence.Join(newObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.15f, 1, 0.5f));

            newObject.SetCurrentTile(tile);
            tile.SetTileObject(newObject);
            LogSpawnedObject(newObject, tile, logSpawn);
            
            if (sendInterfaceEvent)
            {
                newObject.GetGridObjectClickableActions().ForEach(action => action.OnObjectGenerated(newObject));
            }
        }

        private Vector3 CalculateSpawnPosition(GridObjectDataContainer.GridObjectType objType, GridTile tile)
        {
            Vector3 spawnOffset = Vector3.zero;

            if (_spawnOffsetDictionary.TryGetValue(objType, out var value))
                spawnOffset = value;

            return tile.transform.position + spawnOffset;
        }

        private void LogSpawnedObject(GridObject obj, GridTile tile, bool logSpawn)
        {
            if (logSpawn)
            {
                TDebug.Log($"Spawned {obj.GetGridObjectData().Type} (Level {obj.GetGridObjectData().Level}) at {tile.GridPosition}");
            }
        }
    }
}