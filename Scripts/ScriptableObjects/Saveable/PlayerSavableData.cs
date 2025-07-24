using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = nameof(PlayerSavableData), menuName = "Handler Project/Core/PlayerSavableData", order = 1)]
    public class PlayerSavableData : PersistentSaveManager<PlayerSavableData>, IResettable
    {
        #region Public Variables
        
        [SerializeField] private int _levelIndex;
        
        [SerializeField] private bool isAllTutorialCompleted;
        
        #endregion

        #region PublicProperties

        public int LevelIndex { get => _levelIndex; set => _levelIndex = value; }
        
        public bool IsAllTutorialCompleted { get => isAllTutorialCompleted; set => isAllTutorialCompleted = value; }
        
        #endregion

        #region PUBLIC METHODS

        public bool IsLevelEqualsZero()
        {
            return _levelIndex == 0;
        }

        #endregion
    }
}