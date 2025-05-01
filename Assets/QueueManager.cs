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
    private struct QueueElement
    {
        public string inputString;
        public bool shouldAccept;
    }

    [SerializeField] private float dequeSpeed;
    [SerializeField]private List<QueueElement> queue = new();
    [SerializeField]Queue<InputText> inputTextList = new();
    [SerializeField]private InputText inputTextPrefab;
    [SerializeField] ElementsContainer queueContainer;
    private void Start()
    {
        foreach (var element in queue)
        {
            var inputText = Instantiate(inputTextPrefab, queueContainer.transform);
            inputText.SetInputString(element.inputString, element.shouldAccept);
            inputTextList.Enqueue(inputText);
        }
    }
    
    [ContextMenu("parse inputstring")]
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
        StartCoroutine( StartParsing());
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
    
}
