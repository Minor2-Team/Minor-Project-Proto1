using UnityEngine;
using System;
using System.IO;
using Fare;
public class FareParser : MonoBehaviour
{
    [SerializeField] string regex1 = "ab*";   // Matches 'a' followed by zero or more 'b's
    [SerializeField] string regex2 = "(ab)+"; // Matches one or more occurrences of "ab"

    [ContextMenu("ParseRegex")]
    void GenerateManualDFA()
    {
        string pattern = "0(0|1)*1";  // Matches binary strings that start with '0' and end with '1'

        // Convert regex to a DFA
        Automaton dfa = new RegExp(regex1).ToAutomaton();
        dfa.Determinize();
        dfa.Minimize();
        
        

    }

    void PrintDFA(Automaton dfa)
    {
        
    }
    
    
}
