using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class GameStartHandler : MonoBehaviour
    {
        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            EventManager.InGameEvents.LevelStart?.Invoke();
            
            EventManager.InGameEvents.GameStarted?.Invoke();
        }
    }
}