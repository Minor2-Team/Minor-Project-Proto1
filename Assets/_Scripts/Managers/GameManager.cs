using System;
using _Scripts.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager> {
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    [SerializeField] private int score=0;

    [Header("Game Sounds")] 
    [SerializeField]private AudioClip accept;
    [SerializeField]private AudioClip reject;
    [SerializeField]private AudioClip win;
    [SerializeField]private AudioClip connected;
    [SerializeField]private AudioClip parsed;

    public int acceptedCount;
    public int stringCount;
    public GameState State { get; private set; }

    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState) {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState) {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        
    }

    private void HandleStarting() {
        
    }
    public void TransitionConnected()
    {
        AudioSystem.Instance.PlaySound(connected);
    }
    public void StringParsed()
    {
        AudioSystem.Instance.PlaySound(parsed);
    }
    public void StringAccepted(int length)
    {
        acceptedCount++;
        AudioSystem.Instance.PlaySound(accept);
        score+=length;
        UIManager.Instance.SetScore(score);
        if (acceptedCount >= stringCount)
        {
            SceneLoader.Instance.LoadSceneIndex(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    public void StringRejected()
    {
        AudioSystem.Instance.PlaySound(reject);
        var queueManager = FindFirstObjectByType<QueueManager>();
        queueManager.StopParsing();
        UIManager.Instance.LoadScreen(true);
    }
    public void Reload()
    {
        var queueManager = FindFirstObjectByType<QueueManager>();
        stringCount = queueManager.queue.Count;
        queueManager.Reload();
        score = 0;
        UIManager.Instance.SetScore(score);
    }
    public void MainMenu()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
[Serializable]
public enum GameState {
    Starting = 0,

    Win = 5,
    Lose = 6,
}
