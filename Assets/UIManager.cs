using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]TextMeshProUGUI scoreText;
    [SerializeField]GameObject RetryScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score:"  + score;
    }

    public void LoadScreen(bool val)
    {
        RetryScreen.SetActive(val);
    }
}
