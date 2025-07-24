using _Game.Scripts.General;

namespace _Game.Scripts.Managers.Core
{
    public static class GameStateData
    {
        #region Private Variables

        private static GameState currentGameState;

        #endregion

        #region Public Methods

        public static void ChangeGameState(GameState newGameState)
        {
            currentGameState = newGameState;
        }

        public static GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        #endregion
    }
    
    public enum GameState
    {
        LevelLoaded, 
        LevelStart,
        LevelEnd ,
        EndMetaStart,
        Success,
        Fail,
        BallChase
    }
}