using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Scripts.Managers.Core.GameManager
{
    public class SplashManager : MonoBehaviour
    {
        [SerializeField] private Image blackPanel; 
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private float cursorMoveDuration = 1.0f;
        [SerializeField] private int nextSceneIndex = 1;
        [SerializeField] private Image _loadingFill;

        private AsyncOperation _sceneLoadOperation;

        private void Start()
        {
            FadeInPanel();
            CallSceneInitialization();
        }

        private void FadeInPanel()
        {
            blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, 1);
            blackPanel.DOFade(0, fadeDuration);
        }

        private void FillLoadingBar()
        {
            _loadingFill.DOFillAmount(1, cursorMoveDuration).OnComplete(ActivateScene);
        }

        private void CallSceneInitialization()
        {
            FillLoadingBar();
            StartCoroutine(InitializeSceneLoading());
        }

        private IEnumerator InitializeSceneLoading()
        {
            yield return null;
            StartCoroutine(LoadNextSceneAsync());
        }

        private IEnumerator LoadNextSceneAsync()
        {
            _sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Single);

            if (_sceneLoadOperation == null) yield break;
            _sceneLoadOperation.allowSceneActivation = false;
            yield return new WaitUntil(() => _sceneLoadOperation.progress >= 0.9f);
        }

        private void ActivateScene()
        {
            if (_sceneLoadOperation != null && !_sceneLoadOperation.isDone)
            {
                _sceneLoadOperation.allowSceneActivation = true;
                StartCoroutine(UnloadCurrentScene());
            }
            else
            {
                Debug.LogError("Scene load operation not ready or already completed.");
            }
        }

        private IEnumerator UnloadCurrentScene()
        {
            yield return new WaitUntil(() => _sceneLoadOperation.isDone);
            FadeOutPanel();
        }

        private void FadeOutPanel()
        {
            blackPanel.DOFade(1, fadeDuration).OnComplete(() =>
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            });
        }
    }
}