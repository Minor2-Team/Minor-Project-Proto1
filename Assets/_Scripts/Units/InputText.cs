using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.Units
{
    public class InputText : MonoBehaviour
    {
        [SerializeField]private string inputString;
        [SerializeField]private string parsedString;
        [SerializeField]private int counter;
        [SerializeField]private float smoothnessTiming;
        private TextMeshProUGUI _textMesh;
        private State _currState;

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _currState = FindAnyObjectByType<InitialState>();
            
        }
        
        public void CallParsing()
            {
                print("lol");
                counter = 0;
                parsedString=inputString;
                _textMesh.text = inputString;
                StartCoroutine(StartParsing());
            }
        
            private IEnumerator StartParsing()
            {
                _textMesh.gameObject.SetActive(true);
                _textMesh.transform.DOMove(_currState.transform.position, smoothnessTiming)
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
                    _textMesh.transform.DOMove(_currState.transform.position, smoothnessTiming)
                        .SetEase(Ease.InOutSine);
                    yield return new WaitForSeconds(smoothnessTiming/2);
                    _textMesh.text = parsedString;
                    yield return new WaitForSeconds(smoothnessTiming/2);
                }
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

        public void SetInputString(string str,bool flag)
        {
            _textMesh.text = str;
            inputString = str;
            if (flag) _textMesh.color = Color.red;
        }
    }
}
