using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Scripts.Units
{
    public class InputText : MonoBehaviour
    {
        [SerializeField]private string inputString;
        [SerializeField]private string parsedString;
        [SerializeField]private int counter;
        [SerializeField]private float smoothnessTiming;
        public int stringLength;
        private TextMeshProUGUI _textMesh;
        private State _currState;
        private bool _shouldNotAccept;

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
                    GameManager.Instance.StringParsed();
                    if (parsedString.Length==0)
                    {
                        if (_currState is FinalState)
                        {
                            
                            
                            if (_shouldNotAccept)
                            {
                                GameManager.Instance.StringRejected();
                            
                            }
                            else
                            {
                                print("String is accepted");
                                GameManager.Instance.StringAccepted(stringLength);
                            }
                        }
                        else
                        {
                            if (_shouldNotAccept)
                            {
                                GameManager.Instance.StringAccepted(stringLength);
                            }
                            else
                            {
                                print("String is not accepted");
                                GameManager.Instance.StringRejected();
                            }
                            
                        }
                        Destroy(gameObject);
                        yield break;
                    }
                    if (!_currState.transitions.TryGetValue(PeekStringChar(), out var nextTransition))
                    {
                        print(PeekStringChar());
                        
                        if (_shouldNotAccept)
                        {
                            GameManager.Instance.StringAccepted(stringLength);
                            
                        }
                        else
                        {
                            print("Dead State Reached");
                            GameManager.Instance.StringRejected();
                        }
                        Destroy(gameObject);
                        yield break;
                    }
                    _currState = nextTransition.toState;
                    ParseStringChar();
                    _textMesh.transform.DOMove(_currState.transform.position, smoothnessTiming)
                        .SetEase(Ease.InOutSine);
                    yield return new WaitForSeconds(smoothnessTiming/2);
                    _textMesh.text = parsedString;
                    if (parsedString.Length == 0)
                    {
                        if (_currState is FinalState)
                        {


                            if (_shouldNotAccept)
                            {
                                GameManager.Instance.StringRejected();

                            }
                            else
                            {
                                print("String is accepted");
                                GameManager.Instance.StringAccepted(stringLength);
                            }
                        }
                        Destroy(gameObject);
                    }

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
            stringLength = str.Length;
            _shouldNotAccept = flag;
            if (flag) _textMesh.color = Color.red;
        }
    }
}
