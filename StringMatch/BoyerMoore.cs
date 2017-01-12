using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    public class BoyerMoore
    {
        // Scan the pattern right to left and match with the input text.
        // calculate how much to skip if there is a mismatch
        private string pattern = "needle";
        private int r = 26; // number characters in ascii (domain of input text)
        private int[] skipTable = null;

        public void BuildSkip()
        {
            skipTable = new int[r];
            for (int c = 0; c < r; c++)
            {
                skipTable[c] = -1;
            }
            for (int j = 0; j < pattern.Length; j++)
            {
                // skip that many characters on the input sequence
                skipTable[pattern.ElementAt(j)] = j;
            }
        }

        public int Search(string text)
        {
            int N = text.Length;
            int M = pattern.Length;
            int skip = 0;
            for (int i = 0; i <= N-M; i += skip)
            {
                skip = 0;
                for (int j = M-1; j >= 0; j--)
                {
                    //  if there is a mismtach then calculate the skip using that table minus where we 
                    // are on the pattern check. 
                    if (pattern.ElementAt(j) != text.ElementAt(i + j))
                    {
                        skip = Math.Max(1, j - skipTable[text.ElementAt(i + j)]);
                        break;
                    }
                }
                if (skip == 0)
                {
                    return i;
                }
            }
            return N;
        }

    }
}
