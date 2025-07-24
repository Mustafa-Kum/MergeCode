using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.StoreManager
{
    public class InitializeUnityGameServices : MonoBehaviour {
    
        //Unity Game Service Status for IAP
        public string environment = "production";
 
        async void Start() {
            try {
                var options = new InitializationOptions()
                    .SetEnvironmentName(environment);
 
                await UnityServices.InitializeAsync(options);
            }
            catch (Exception exception) {
                Debug.LogError("An error occurred during initialization: " + exception.Message);
            }
        }
    }
}