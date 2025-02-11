using UnityEngine;

[CreateAssetMenu(fileName = "UIData", menuName = "Scriptable Objects/UIData")]
public class UIData : ScriptableObject
{
    [SerializeField] int score;
    public int Score { get => score; 
        set
    {
        score = value;
    } 
    }
}
