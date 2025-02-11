using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Systems
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private GameObject loadingCanvas;
        [SerializeField] private float progress = 0;
        public void LoadScene<T>(T scene)
        {
            string sceneName = scene is int 
                ? SceneManager.GetSceneByBuildIndex(Convert.ToInt32(scene)).name 
                : scene.ToString();

            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            loadingCanvas.SetActive(true);
            
            var scene = SceneManager.LoadSceneAsync(sceneName);
            
            
            scene.allowSceneActivation = false;

            do
            {
                progress = scene.progress;
                if (scene.progress >= 0.9f)
                {
                    scene.allowSceneActivation = true;

                    loadingCanvas.SetActive(false);
                }
                yield return null;
            } while (!scene.isDone);

        }
        
    }
}

public enum SceneIndex
{
    Menu,
    Main
}