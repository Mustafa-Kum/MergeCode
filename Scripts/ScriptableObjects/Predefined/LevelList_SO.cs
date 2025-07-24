using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.ScriptableObjects.Saveable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = nameof(LevelList_SO), menuName = "Handler Project/Core/LevelListSO", order = 1)]
    public class LevelList_SO : ScriptableObject
    {
        #region Public Variables
    
        [SerializeField]
        private List<Level_SO> _allLevels;
    
        [SerializeField]
        private PlayerSavableData _playerVariable;

        [SerializeField]
        private List<int> _excludeLevelsIndices = new List<int>();
    
        #endregion

        #region Properties

        public List<Level_SO> AllLevels => _allLevels.Except(_allLevels.Where((level, index) => _excludeLevelsIndices.Contains(index))).ToList();

        #endregion
    
        #region Public Methods

        public Level_SO GetLevelWithIndex(int currentLevel)
        {
            var availableLevels = AllLevels;
            if (availableLevels.Count == 0)
            {
                Debug.LogWarning("The level list is empty or all levels are excluded.");
                return null;
            }

            int index = currentLevel % availableLevels.Count;
            return availableLevels[index];
        }

        public Level_SO GetCurrentLevelSO()
        {
            return GetLevelWithIndex(GetActualLevel());
        }

        public int GetActualLevel()
        {
            return _playerVariable.LevelIndex % AllLevels.Count;
        }

        public bool IsAllLevelsCompleted()
        {
            return _playerVariable.LevelIndex >= AllLevels.Count;
        }

        [Button]
        public void AddExcludeLevel(int levelIndex)
        {
            if (!_excludeLevelsIndices.Contains(levelIndex))
            {
                _excludeLevelsIndices.Add(levelIndex);
            }
        }
    
        #endregion
    }
}
