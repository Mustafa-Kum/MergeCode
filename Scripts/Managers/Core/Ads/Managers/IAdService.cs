using System;

namespace _Game.Scripts.Managers.Core
{
    public interface IAdService
    {
        void Initialize();
        void Uninitialize();
        void ShowRewardedAd(Action onAdSuccessful);
        void ShowInterstitialAd();
        void ShowBannerAd();        
    }
}