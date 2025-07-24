using System;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = nameof(Level_SO), menuName = "Handler Project/Core/LevelSO", order = 2)]
    public class Level_SO : ScriptableObject
    {
        #region Public Variables

        [SerializeField] private GameObject _levelPrefab;
        public InLevelObjectsConfigurationsData inLevelObjectsConfigurationsData;
        public bool isTutorialLevel;
        
        #endregion
        
        #region Properties

        public GameObject LevelPrefab => _levelPrefab;

        #endregion
    }

    [Serializable]
    public class InLevelObjectsConfigurationsData
    {
        [SerializeField] private GameObject _backgroundPrefab;
        [SerializeField] private GameObject _objectSpawn;
        [SerializeField] private GameObject _pathFollower;
        [SerializeField] private GameObject _levelParticle;
        [SerializeField] private GameObject _levelObstacle;
        [SerializeField] private Texture2D _cubeTexture;

        #region Properties

        public GameObject BackgroundPrefab => _backgroundPrefab;
        public GameObject ObjectSpawn => _objectSpawn;
        public GameObject EndCharacter => _pathFollower;
        public GameObject LevelParticle => _levelParticle;
        public GameObject LevelObstacle => _levelObstacle;
        public Texture2D CubeTexture => _cubeTexture;
        
        #endregion
    }
}
