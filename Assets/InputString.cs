using System;
using System.Collections;
using _Scripts.Units;
using UnityEngine;

public class InputString : MonoBehaviour
{
    [SerializeField]private string inputString="ababab";
    [SerializeField]private int counter;

    private State _currState;
    private State _state;
    private void Awake()
    {
        counter = inputString.Length - 1;
    }

    void Start()
    {
        _currState = FindAnyObjectByType<InitialState>();

        StartCoroutine(StartParsing());
    }

    private IEnumerator StartParsing()
    {
        while (true)
        {
            if (counter < 0)
            {
                if (_currState is FinalState)
                    print("String is accepted");
                else
                {
                    
                    print("String is not accepted");
                    break;
                }
            }
            if (!_currState.transitions.TryGetValue(PeekStringChar(), out var nextTransition))
            {
                
                break;
            }

            
            _currState = nextTransition.to;
            ParseStringChar();
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        
    }
    public char PeekStringChar()
    {
        return inputString[counter];
    }

    public char ParseStringChar()
    {
        return inputString[counter--];
    }
}
