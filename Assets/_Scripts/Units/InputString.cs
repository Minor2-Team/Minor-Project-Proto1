using System;
using System.Collections;
using _Scripts.Units;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InputString : MonoBehaviour
{
    [SerializeField]private string inputString="ababab";
    [SerializeField]private string parsedString;
    [SerializeField]private int counter;
    [SerializeField]private TextMeshProUGUI inputStringText;
    [SerializeField]private float smoothnessTiming;
    private State _currState;
    private State _state;
    private void Awake()
    {
        parsedString=inputString;
        inputStringText.text = inputString;
    }

    private void Start()
    {
        _currState = FindAnyObjectByType<InitialState>();
        inputStringText.transform.position = _currState.transform.position;
    }

    [ContextMenu("StartParsing")]
    public void CallParsing()
    {
        counter = 0;
        parsedString=inputString;
        inputStringText.text = inputString;
        StartCoroutine(StartParsing());
    }

    private IEnumerator StartParsing()
    {
        inputStringText.gameObject.SetActive(true);
        _currState = FindAnyObjectByType<InitialState>();
        inputStringText.transform.DOMove(_currState.transform.position, smoothnessTiming)
            .SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (parsedString.Length==0)
            {
                if (_currState is FinalState)
                {
                    
                    print("String is accepted");
                    yield break;
                }
                else
                {
                    
                    print("String is not accepted");
                    yield break;
                }
            }
            if (!_currState.transitions.TryGetValue(PeekStringChar(), out var nextTransition))
            {
                print(PeekStringChar());
                print("Dead State Reached");
                yield break;
            }

            print("Transition from " + _currState.stateName + " to " + nextTransition.toState.stateName + " on " + PeekStringChar());
            _currState = nextTransition.toState;
            
            ParseStringChar();
            inputStringText.transform.DOMove(_currState.transform.position, smoothnessTiming)
                .SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(smoothnessTiming/2);
            inputStringText.text = parsedString;
            yield return new WaitForSeconds(smoothnessTiming/2);
        }
    }

    void Update()
    {
        
    }
    public char PeekStringChar()
    {
        return parsedString[counter];
    }

    public char ParseStringChar()
    {
        parsedString = parsedString.Remove(counter, 1);
        return inputString[counter];
    }
}
