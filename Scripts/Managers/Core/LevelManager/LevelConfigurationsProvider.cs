using _Game.Scripts.ScriptableObjects.Predefined;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class LevelConfigurationsProvider
    {
        #region Public Variables

        private readonly InLevelObjectsConfigurationsData _inLevelObjectsConfigurationsData;
        private readonly Transform _levelHolder;

        #endregion

        #region Private Variables

        private GameObject _instantiatedBackground;
        private GameObject _instantiatedPathFollower;
        private GameObject _instantiatedParticle;

        #endregion

        #region Constructors

        public LevelConfigurationsProvider(InLevelObjectsConfigurationsData inLevelObjectsConfigurationsData, Transform levelHolder)
        {
            _inLevelObjectsConfigurationsData = inLevelObjectsConfigurationsData;
            _levelHolder = levelHolder;
        }
        
        #endregion

        #region Public Methods

        public void InitializeLevelConfigurations()
        {
            InstantiateBackground(_inLevelObjectsConfigurationsData.BackgroundPrefab);
            InstantiateLevelSpecifiedParticle(_inLevelObjectsConfigurationsData.LevelParticle);
            //InstantiatePathFollower(_inLevelObjectsConfigurationsData.EndCharacter);
        }

        #endregion
        

        #region Private Methods

        private void InstantiateLevelSpecifiedParticle(GameObject particlePrefab)
        {
            if (particlePrefab != null)
            {
                _instantiatedParticle = Object.Instantiate(particlePrefab, particlePrefab.transform.position, particlePrefab.transform.rotation);
                _instantiatedParticle.transform.SetParent(_levelHolder);
            }
        }

        private void InstantiateBackground(GameObject backgroundPrefab)
        {
            if (backgroundPrefab != null)
            {
                _instantiatedBackground = Object.Instantiate(backgroundPrefab, backgroundPrefab.transform.position, backgroundPrefab.transform.rotation);
                var position = _instantiatedBackground.transform.position;
                position = new Vector3(position.x, position.y, position.z);
                _instantiatedBackground.transform.position = position;
                _instantiatedBackground.transform.localScale = backgroundPrefab.transform.localScale;
                _instantiatedBackground.transform.SetParent(_levelHolder);
            }
        }

        private void InstantiatePathFollower(GameObject pathFollowerPrefab)
        {
            if (pathFollowerPrefab != null)
            {
                _instantiatedPathFollower = Object.Instantiate(pathFollowerPrefab, pathFollowerPrefab.transform.position, pathFollowerPrefab.transform.rotation);
                _instantiatedPathFollower.transform.localScale = pathFollowerPrefab.transform.localScale;
                _instantiatedPathFollower.transform.SetParent(_levelHolder);
            }
        }

        #endregion
    }
}