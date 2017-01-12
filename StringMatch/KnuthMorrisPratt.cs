using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    public class KnuthMorrisPratt
    {

        public string Pattern = "ababac";
        public int[,] dfa = null;

        public void BuildDFA()
        {
            // below should be abc
            var patternDistinct = Pattern.Distinct().OrderBy(xObj => xObj.GetHashCode()).ToString();
            int x = patternDistinct.Count();
            int y = Pattern.Count();
            dfa = new int[x, y];
            dfa[0, 0] = 1; // So that j can start one ahead of X
            for (int X = 0, j = 1 ; j < y ; j++)
            {
                for (int c = 0; c < x; c++)
                {
                    dfa[c, j] = dfa[c, X]; // Copy the mismatch case by placing X to be one behind the J.
                }
                
                // match the exact char from the pattern
                dfa[patternDistinct.IndexOf(Pattern.ElementAt(j)), j] = j + 1;

                // Update state X to take a transition based on character seen by j (the pointer ahead of it)
                X = dfa[patternDistinct.IndexOf(Pattern.ElementAt(j)), X];
            }
        }
        
        public int Search(string text)
        {
            int i = 0, j = 0, N = text.Length;
            for (; i < N && j < Pattern.Length; i++)
            {
                j = dfa[text.ElementAt(i), j];
            }
            if (j == Pattern.Length)
            {
                return i - Pattern.Length;
            }
            return N;
        }

    }
}
