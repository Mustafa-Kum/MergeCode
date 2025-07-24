using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.Managers.Core
{
    public class LevelDebugger : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] 
        private Button nextLevelButton;

        [SerializeField] 
        private Button previousLevelButton;
        
        [SerializeField]
        private PlayerSavableData playerSavableData;
        
        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            nextLevelButton.onClick.AddListener(GoToNextLevel);
            previousLevelButton.onClick.AddListener(GoToPreviousLevel);

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region PRIVATE METHODS

        private void GoToNextLevel()
        {
            playerSavableData.LevelIndex++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GoToPreviousLevel()
        {
            playerSavableData.LevelIndex--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion


    }
}