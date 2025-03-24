using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.Units
{
    public class State : MonoBehaviour
    {
        [SerializeField] public string stateName;
        [SerializeField]public Dictionary<char,Transition> transitions=new ();
        [SerializeField] public float radius=1.5f;
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI stateLable;

        public virtual  void Awake()
        {
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera=Camera.main;
            stateLable = canvas.GetComponentInChildren<TextMeshProUGUI>();
            stateLable.text=stateName;
        }
    }
}
