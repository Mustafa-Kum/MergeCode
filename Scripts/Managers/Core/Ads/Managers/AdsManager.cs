using System;
using _Game.Scripts.Helper.Extensions.System;
using UnityEngine;

namespace _Game.Scripts.Managers.Core
{
    public class AdsManager : MonoBehaviour
    {
        private IAdService adService;

        private void Awake() => CreateInstanceAdService();

        private void OnEnable()
        {
            EventManager.AdEvents.ShowRewarded += ShowRewardedAd;
            EventManager.AdEvents.ShowInterstitial += ShowInterstitialAd;
            EventManager.AdEvents.ShowBanner += ShowBannerAd;
        }
        
        private void OnDisable()
        {
            EventManager.AdEvents.ShowRewarded -= ShowRewardedAd;
            EventManager.AdEvents.ShowInterstitial -= ShowInterstitialAd;
            EventManager.AdEvents.ShowBanner -= ShowBannerAd;
            
            adService?.Uninitialize();
        }
        
        private void CreateInstanceAdService()
        {
            //adService.Initialize();
        }

        private void ShowRewardedAd(Action callback)
        {
            if (adService == null)
            {
                TDebug.LogWarning("Ad Service not initialized.");
                return;
            }

            adService.ShowRewardedAd(callback);
        }

        private void ShowInterstitialAd()
        {
            if (adService == null)
            {
                TDebug.LogWarning("Ad Service not initialized.");
                return;
            }

            adService.ShowInterstitialAd();
        }
        
        private void ShowBannerAd()
        {
            if (adService == null)
            {
                TDebug.LogWarning("Ad Service not initialized.");
                return;
            }

            adService.ShowBannerAd();
        }
    }
}
