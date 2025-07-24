using System;
using _Game.Scripts.Helper.Extensions.System;

namespace _Game.Scripts.Managers.Core.Ads.AdMediations
{
    sealed class UnityAdsService : IAdService
    {
        private Action _onAdSuccessful;

        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public void ShowRewardedAd(Action onAdSuccessful)
        {
            TDebug.LogWarning("Unity Ads Service: ShowRewardedAd(Action onAdSuccessful) is not implemented. Debug mode is on. ");
            
            _onAdSuccessful = onAdSuccessful;
            
            #if UNITY_EDITOR
            _onAdSuccessful?.Invoke();
            #endif
        }

        public void ShowInterstitialAd()
        {
            // Implement Unity Ads logic for showing interstitial ads
        }
        
        public void ShowBannerAd()
        {
            // Implement Unity Ads logic for showing banner ads
        }
    }
}