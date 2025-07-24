using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Managers.Core
{
    public static partial class EventManager
    {
        public static class InGameEvents
        {
            public static UnityAction GameStarted;
            public static UnityAction LoadLevel;
            public static UnityAction<GameObject> LevelLoaded;
            public static UnityAction LevelStart;
            public static UnityAction LevelSuccess;
            public static UnityAction AfterLevelSuccess;
            public static UnityAction LevelFail;
        }
        
        public static class SaveEvents
        {
            public static UnityAction DataSaved;
            public static UnityAction DataLoaded;
        }
        
        public static class AudioEvents
        {
            public static UnityAction<SoundType, bool, bool> AudioPlay;
            public static UnityAction<float> VolumeChange;
            public static UnityAction<bool> AudioEnabled;
        }
        
        public static class AdEvents
        {
            public static UnityAction ShowInterstitial;
            public static UnityAction<Action> ShowRewarded;
            public static UnityAction ShowBanner;
        }
        
        public static class AnalyticsEvents
        {
            public static UnityAction AnalyticsInitialized;
        }
    }
}