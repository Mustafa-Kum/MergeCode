using System.Linq;
using _Game.Scripts.Helper.Extensions.System;
using _Game.Scripts.ScriptableObjects.Saveable;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Game.Scripts.Managers.Core
{
    public class SaveManager : MonoBehaviour
    {
        public InterfaceSerialization<ISaveableProvider>[] _constantSaveables;
        public InterfaceSerialization<ISaveableProvider>[] _runtimeSaveables;
        
        private void Awake()
        {
            LoadAll();
        }

        private void OnEnable()
        {
            EventManager.InGameEvents.LevelSuccess += SavePersistentData;
            EventManager.InGameEvents.LevelLoaded += LevelLoaded;
            EventManager.Resource.CurrencyChanged += OnCurrencyAmountSave;
            EventManager.IAPEvents.PurchaseSuccess += OnPurchaseSave;
            EventManager.IAPEvents.BundlePurchased += OnPurchaseSave;
        }


        private void OnDisable()
        {
            EventManager.InGameEvents.LevelSuccess -= SavePersistentData;
            EventManager.InGameEvents.LevelLoaded -= LevelLoaded;
            EventManager.Resource.CurrencyChanged -= OnCurrencyAmountSave;
            EventManager.IAPEvents.PurchaseSuccess -= OnPurchaseSave;
            EventManager.IAPEvents.BundlePurchased -= OnPurchaseSave;
        }
        
        private void SavePersistentData()
        {
            // Delayed save for the first item
            DOVirtual.DelayedCall(0.1f, () => _constantSaveables[0].I.SaveData());

            // Instant save for the rest
            _constantSaveables.Skip(1).ToList().ForEach(s => s.I.SaveData());
        }
        
        private void OnPurchaseSave(PurchaseEventArgs args)
        {
            SavePersistentData();
        }
        
        private void LevelLoaded(GameObject level)
        {
            //PlayerData
            _constantSaveables[0].I.SaveData();
        }

        private void OnCurrencyAmountSave(CurrencyType type)
        {
            _constantSaveables[1].I.SaveData();
        }
        
        [Button]
        void SaveAll()
        {
            foreach(var saveable in _constantSaveables)
            {
                saveable.I.SaveData();
            }
            
            SaveRuntime();

        }
        
        [Button]
        void LoadAll()
        {
            foreach (var saveable in _constantSaveables)
            {
                saveable.I.LoadData();
            }
            
            LoadRuntime();
      
        }


        [Button]
        void SaveRuntime()
        {
            foreach(var saveable in _runtimeSaveables)
            {
                saveable.I.SaveData();
            }
        }
        
        [Button]
        void LoadRuntime()
        {
            foreach (var saveable in _runtimeSaveables)
            {
                saveable.I.LoadData();
            }
        }

#if UNITY_EDITOR
        [Button][GUIColor( 0.8f, 0.3f, 0.3f, 1f)]
        void ClearPersistentDataPath()
        {
            TDebug.LogGreen("Persistent Data Cache and PlayerPrefs Cleared");
            ES3.DeleteFile();
            PlayerPrefs.DeleteAll();
        }
#endif
    }
}