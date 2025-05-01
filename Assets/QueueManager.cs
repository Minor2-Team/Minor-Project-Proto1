using System.Collections;
using System.Collections.Generic;
using _Scripts.Units;
using Akassets.SmoothGridLayout;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class QueueManager : MonoBehaviour
{
    [System.Serializable]
    public struct QueueElement
    {
        public string inputString;
        public bool shouldNotAccept;
    }

    [SerializeField] private float dequeSpeed;

    [SerializeField] public List<QueueElement> queue = new();
    [SerializeField]Queue<InputText> inputTextList = new();
    [SerializeField]private InputText inputTextPrefab;
    [SerializeField] ElementsContainer queueContainer;
    
    Coroutine _parsingCoroutine;
    private void Start()
    {
        GameManager.Instance.stringCount = queue.Count;
        GameManager.Instance.acceptedCount = 0;
        foreach (var element in queue)
        {
            var inputText = Instantiate(inputTextPrefab, queueContainer.transform);
            inputText.SetInputString(element.inputString, element.shouldNotAccept);
            inputTextList.Enqueue(inputText);
        }
    }
    
    [ContextMenu("parse input String")]
    public void ParseLastString()
    {
        if (!inputTextList.TryPeek(out var endElement)) return;
        inputTextList.Dequeue();
        endElement.transform.SetParent(endElement.transform.parent.parent);
        endElement.CallParsing();

    }
    [ContextMenu("parse all strings")]
    public void ParseAllStrings()
    {
        _parsingCoroutine = StartCoroutine( StartParsing());
    }
    public IEnumerator StartParsing()
    {
        while (inputTextList.TryPeek(out var endElement))
        {
            inputTextList.Dequeue();
            endElement.transform.SetParent(endElement.transform.parent.parent);
            endElement.CallParsing();
            yield return new WaitForSeconds(dequeSpeed);
        }
    }

    public void StopParsing()
    {
        if(_parsingCoroutine!=null)
            StopCoroutine(_parsingCoroutine);
        _parsingCoroutine = null;
    }
    public void Reload()
    {
        if(_parsingCoroutine!=null)
            StopCoroutine(_parsingCoroutine);
        _parsingCoroutine = null;
        while (inputTextList.Count > 0)
        {
            var element = inputTextList.Dequeue();
            if (element)
            {
                Destroy(element.gameObject);
            }
        }
        foreach (var element in queue)
        {
            var inputText = Instantiate(inputTextPrefab, queueContainer.transform);
            inputText.SetInputString(element.inputString, element.shouldNotAccept);
            print(inputText);
            inputTextList.Enqueue(inputText);
        }
    }
}
