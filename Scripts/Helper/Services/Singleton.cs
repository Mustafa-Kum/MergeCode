using UnityEngine;

namespace _Game.Scripts.Helper.Services
{
    /// <summary>
    /// Generic Singleton Class for Unity MonoBehaviour.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went very wrong " +
                                           " - there should never be more than 1 singleton! Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            DontDestroyOnLoad(singletonObject);

                            Debug.Log("[Singleton] An instance of " + typeof(T) +
                                      " is needed in the scene, so '" + singletonObject +
                                      "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +
                                      _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that.
        /// </summary>
        protected void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}
