using System;
using UnityEngine;
using Microsoft.Automata;



public class RegexParser : MonoBehaviour
{
    [SerializeField]string regex1 = "ab*";  // Matches 'a' followed by zero or more 'b's
    [SerializeField]string regex2 = "(ab)+";  // Matches 'a' followed by at least one 'b' or nothing
    
    void MakeAutometa()
    {
        var solver = new CharSetSolver();

        
        // Define DFA transitions
        var transitions = new Move<BDD>[]
        {
            new(0, 0, solver.MkCharConstraint('a')), // q0 --a--> q1
            
        };
        int[] finalstates = { 0 };

        // Create DFA
        Automaton<BDD> dfa1 = Automaton<BDD>.Create(
            solver,                   // First parameter must be the solver
            0,                         // Start state (int)
            finalstates,           // Final states
            transitions                // Transitions (Move<BDD>[])
        );


        // Convert DFA to Regex
        Automaton<BDD> minimizedDFA = dfa1; // Minimize the DFA;
        
        
                // Define two regex patterns
        
                // Convert regex to DFAs
                Automaton<BDD> dfa2 = solver.Convert(regex2).Determinize().MinimizeClassical(0,true);
                print(regex2);
                Move<BDD> temp=new(0,0,solver.MkCharConstraint('.'));
                foreach (var move in dfa2.GetMoves())
                {
                    print($"({move.SourceState}) -- {move.Label} --> ({move.TargetState})");
                    
                }
                print("--------------");
                foreach (var move in minimizedDFA.GetMoves())
                {
                    print($"({move.SourceState}) -- {move.Label} --> ({move.TargetState})");
                }
                // Compare the DFAs
                bool areEquivalent = dfa1.IsEquivalentWith(dfa2);
                print(areEquivalent);
                
                

    }

   

    
}
