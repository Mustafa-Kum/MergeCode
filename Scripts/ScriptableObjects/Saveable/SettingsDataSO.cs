using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = nameof(SettingsDataSO), menuName = "Handler Project/Core/SettingsDataSO", order = 0)]
    public class SettingsDataSO : PersistentSaveManager<SettingsDataSO>, IResettable
    {
        #region Public Variables

        public bool IsSoundEnabled;
        public bool IsVibrationEnabled;

        #endregion
    }
}