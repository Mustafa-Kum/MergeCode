using UnityEngine;
namespace _Game.Scripts.ScriptableObjects.Predefined
{
    [CreateAssetMenu(fileName = nameof(AudioClipData), menuName = "Handler Project/Audio/AudioClipData", order = 1)]
    public class AudioClipData : ScriptableObject
    {
        [SerializeField]
        private AudioClip audioClip;
        public AudioClip AudioClip
        {
            get { return audioClip; }
            set { audioClip = value; }
        }

        [SerializeField]
        private float volume = 1.0f;
        public float Volume
        {
            get { return volume; }
            set { volume = Mathf.Clamp(value, 0.0f, 1.0f); } // Clamp to ensure volume is between 0 and 1
        }

        [SerializeField]
        private float pitch = 1.0f;
        public float Pitch
        {
            get { return pitch; }
            set { pitch = Mathf.Clamp(value, 0.1f, 3.0f); } // Adjust min/max values as needed
        }
        
        [SerializeField]
        private bool shouldLoop = false;
        
        public bool ShouldLoop
        {
            get { return shouldLoop; }
            set { shouldLoop = value; }
        }

        // Add other properties as needed
    }
}
