using System.Collections.Generic;
using _Game.Scripts._GameLogic.Data.Product;
using AssetKits.ParticleImage;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.UI
{
    [CreateAssetMenu(fileName = nameof(CurrencyParticleImageDataContainer), menuName = "Merge Valley/Data/Currency Particle Image Data Container", order = 0)]
    public class CurrencyParticleImageDataContainer : SerializedScriptableObject
    {
        public Dictionary<CurrencyType, ParticleImage> CurrencyParticles;
        public Dictionary<GridObjectProductDataContainer.ProductType, ParticleImage> ProductParticles;
        public ParticleImage MergeParticle;
        
        public ParticleImage GetCurrencyParticle(CurrencyType type)
        {
            if (CurrencyParticles.TryGetValue(type, out var particle)) return particle;
            Debug.LogError($"Currency Particle for {type} is not found in the CurrencyParticles dictionary.");
            return null;
        }
        
        public ParticleImage GetProductParticle(GridObjectProductDataContainer.ProductType type)
        {
            if (ProductParticles.TryGetValue(type, out var particle)) return particle;
            Debug.LogError($"Product Particle for {type} is not found in the ProductParticles dictionary.");
            return null;
        }
        
        public ParticleImage GetMergeParticle()
        {
            return MergeParticle;
        }
    }
}