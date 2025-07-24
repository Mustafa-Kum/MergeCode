using UnityEngine;

namespace Handler.Extensions
{
    public static class HandlerGameObject
    {
        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }
        
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out T requested))
            {
                return requested;
            }
            return gameObject.AddComponent<T>();
        }

        public static void SetActiveSafe(this GameObject self, bool value)
        {
            if (self.activeSelf != value)
            {
                self.SetActive(value);
            }
        }
        
        public static bool FindObjectOfType<T>(out T founded) where T : Object
        {
            founded = Object.FindAnyObjectByType<T>();
            return founded != null;
        }
        
        public static bool FindObjectsOfType<T>(out T[] founded) where T : Object
        {
            founded = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
            return founded != null;
        }
    }
}