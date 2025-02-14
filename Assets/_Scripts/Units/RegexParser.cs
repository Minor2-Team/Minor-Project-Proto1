using UnityEngine;
using Fare;
using System.Collections.Generic;

public class RegexParser : MonoBehaviour
{
    public void AnalyzeRegex(string pattern)
    {
        Debug.Log($"Analyzing regex pattern: {pattern}");
        
        try
        {
            var regExp = new RegExp(pattern);
            var automaton = regExp.ToAutomaton();
            
            // Print basic info
            Debug.Log($"Pattern: {pattern}");
            
            string[] testStrings = { "a", "aa", "ab", "abc", "" };
            foreach (string test in testStrings)
            {
                bool accepts = automaton.Run(test);
                Debug.Log($"String '{test}' is {(accepts ? "accepted" : "rejected")}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing regex: {e.Message}");
        }
    }

    void Start()
    {
        
        AnalyzeRegex("a*b");
    }

    //parsing only
}