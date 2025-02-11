using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Systems
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private GameObject loadingCanvas;
        [SerializeField] private Image progressBar;
        [SerializeField] private float smoothTime;
        private float _target;

        public void LoadScene<T>(T scene)
        {
            string sceneName = scene is int
                ? SceneManager.GetSceneByBuildIndex(Convert.ToInt32(scene)).name
                : scene.ToString();

            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            _target = 0;
            progressBar.fillAmount = 0;
            loadingCanvas.SetActive(true);
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene!.allowSceneActivation = false;
            yield return new WaitForSeconds(0.2f);



            do
            {
                _target = scene.progress;
                if (scene.progress >= 0.9f)
                {
                    _target = 1;

                    scene.allowSceneActivation = true;
                }

                yield return null;
            } while (!scene.isDone);

            yield return new WaitForSeconds(0.2f);
            loadingCanvas.SetActive(false);
        }

        private void Update()
        {
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, _target, smoothTime * Time.deltaTime);
        }

    }

public enum SceneIndex
{
    Menu,
    Main
}
}
