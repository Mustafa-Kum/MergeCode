using UnityEngine;

namespace _Game.Scripts.Helper.Extensions.System
{
    public static class ComponentExtensions
    {
        public static void RemoveComponentInChildren<T>(this Transform transform) where T : Component
        {
            T component = transform.GetComponent<T>();
            if (component != null)
            {
                Object.DestroyImmediate(component);
            }

            foreach (Transform child in transform)
            {
                child.RemoveComponentInChildren<T>();
            }
        }
    }
}