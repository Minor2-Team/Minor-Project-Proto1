using _Scripts.Systems;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneLoader.Instance.LoadScene(SceneIndex.Main);
        });
        
        quitButton.onClick.AddListener(Application.Quit);
        
    }
}
