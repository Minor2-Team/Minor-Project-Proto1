using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleCharacterInputField : MonoBehaviour,IPointerClickHandler
{
    public Action<bool,char> OnCharacterChanged;
    private string chachedText ="";
    [SerializeField] private TMP_InputField inputField;
    
    HashSet<char> Chars = new HashSet<char>();

    private bool canEnterChar = true;
    private bool _skipcall;
    private void Awake()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(HandleInputChanged);
        inputField.onSelect.AddListener(HandleInputSelected);
    }

    private void HandleInputSelected(string arg0)
    {
        inputField.caretPosition = inputField.text.Length;
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }

    private void HandleInputChanged(string text)
    {
        if (_skipcall)
        {
            _skipcall = false;
            return;
        }
        
        if (text.Length >= chachedText.Length)// added
        {
            char lastChar = text[^1];
            if (lastChar == ',')
            {
                if (canEnterChar)
                {
                    _skipcall = true;
                    inputField.text = chachedText;
                }
                canEnterChar = true;
            }
            else if(char.IsWhiteSpace(lastChar))
            {
                
            }
            else
            {
                if (canEnterChar && Chars.Add(lastChar))
                {
                    OnCharacterChanged?.Invoke(true,lastChar);
                    canEnterChar = false;
                }
                else
                {
                    _skipcall = true;
                    inputField.text = chachedText;
                }
            }
        }
        else //  removed
        {
            char lastChar = chachedText[^1];
            if (lastChar == ',')
            {
                canEnterChar = false;
            }
            else
            {
                canEnterChar = true;
                Chars.Remove(lastChar);
                OnCharacterChanged?.Invoke(false,lastChar);
            }
        }
        

        print(canEnterChar);
        chachedText = inputField.text;
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(HandleInputChanged);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.caretPosition = inputField.text.Length;
        inputField.selectionAnchorPosition = inputField.text.Length;
        inputField.selectionFocusPosition = inputField.text.Length;
    }
}
