using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Menu_View;

namespace Loader
{

    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        private float _progress = 0;

        public Action<float> OnSceneLoaded;
        
        public void LoadSceneAsync(int sceneIndex)
        {
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
        }
        

        private IEnumerator LoadSceneCoroutine(int indexScene)
        {
            var loadOperation = SceneManager.LoadSceneAsync(indexScene);
            loadOperation.allowSceneActivation = false;

            _progress = 0f;
            var targetProgress = 0f;
            float minimumLoadTime = 1f;
            float elapsedTime = 0f;

            while (loadOperation.progress < 0.9f || elapsedTime < minimumLoadTime)
            {
                elapsedTime += Time.deltaTime;

                targetProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                _progress = Mathf.MoveTowards(_progress, targetProgress, Time.deltaTime * 0.5f);
                OnSceneLoaded?.Invoke(_progress); 

                yield return null;
            }

            _progress = 1f;
            OnSceneLoaded?.Invoke(_progress * 100);
            yield return new WaitForSeconds(0.5f);

            loadOperation.allowSceneActivation = true;
        }
    }
}