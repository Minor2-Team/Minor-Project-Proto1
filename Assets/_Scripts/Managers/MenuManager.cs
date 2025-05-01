using _Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadSceneIndex(SceneManager.GetActiveScene().buildIndex+1);
        });
        
        quitButton.onClick.AddListener(Application.Quit);
        
    }
}
