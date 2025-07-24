using System.Collections.Generic;
using _Game.Scripts.ScriptableObjects.RunTime;
using UnityEngine;

namespace _Game.Scripts.ScriptableObjects.Saveable
{
    [CreateAssetMenu(fileName = nameof(IAPBundleValuesSO), menuName = "Handler Project/IAP/IAPBundleValuesSO", order = 0)]
    public class IAPBundleValuesSO : PersistentSaveManager<IAPBundleValuesSO>, IResettable
    {
        #region PUBLIC VARIABLES

        public bool noAdsPurchased;
        
        #endregion

        #region PRIVATE VARIABLES

        [HideInInspector] public const string NoAdsBundleId = "com.cubixpath.noads";

        #endregion

        #region PROPERTIES

        public bool NoAdsPurchased => noAdsPurchased;

        #endregion

        #region PRIVATE VARIABLES

        private readonly Dictionary<string, bool> _purchasedBundles = new Dictionary<string, bool>()
        {
            {NoAdsBundleId, false}
        };

        #endregion
        
        #region PUBLIC METHODS
        
        public void WriteBundle(string bundleId)
        {
            if (_purchasedBundles.ContainsKey(bundleId))
            {
                _purchasedBundles[bundleId] = true;
                UpdatePublicVariables(bundleId, true);
            }
            else
            {
                Debug.LogWarning($"Bundle ID '{bundleId}' does not exist in the dictionary.");
            }
        }
        
        public bool IsBundlePurchased(string bundleId)
        {
            return _purchasedBundles.ContainsKey(bundleId) && _purchasedBundles[bundleId];
        }

        #endregion

        #region PRIVATE METHODS

        private void UpdatePublicVariables(string bundleId, bool value)
        {
            switch (bundleId)
            {
                case NoAdsBundleId:
                    noAdsPurchased = value;
                    break;
                default:
                    Debug.LogWarning($"Unhandled bundle ID: {bundleId}");
                    break;
            }
        }

        #endregion
    }
}