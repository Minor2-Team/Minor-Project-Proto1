using UnityEngine;
using Fare;
using System.Collections.Generic;

public class RegexParser : MonoBehaviour
{
    [SerializeField] private string regex;
    [SerializeField] string[] testStrings = { "a", "aa", "ab", "abc", "" };
    public void AnalyzeRegex(string pattern)
    {
            var regExp = new RegExp(pattern);
            var automaton = regExp.ToAutomaton();
            
            foreach (string test in testStrings)
            {
                bool accepts = automaton.Run(test);
                Debug.Log($"String '{test}' is {(accepts ? "accepted" : "rejected")}");
            }

    }

    void Start()
    {
        
        AnalyzeRegex(regex);
    }

    //parsing only
}
