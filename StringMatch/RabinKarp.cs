using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    public class RabinKarp
    {
        // basic idea is on modular hashing
        // R is the base integer (1, 10s, 100s, 1000s, 10^4s...)
        // M is the cardinality of the pattern
        // ti is the charAt on the text
        // Q is a big prime

        // xi = (t[i] * R ^ M-1) + (t[i+1] * R ^ M-2) + (t[i+2] * R ^ M-3) ..... + (t[i+M-1] * R ^ 0) (mod Q)
        // xi+1 = (xi - (t[i] * R ^ M-1)) * R + (t[i+M]
        // The above helps avoid the extra hashing as the wimdow slides 
        // by reusing the previous has that was valid since there is a lot
        // of overlap of terms between the two

        private int R = 256; // base radix
        private long Q = 997; // some big prime
        private int M = 0;
        private long Rm = 0;
        private long PatternHash = 0;

        // Horners method for degree-M polynomial
        public long HornersHash(string key, int M)
        {
            long h = 0;
            for(int i = 0; i < M; i++)
            {
                h = (R * h + (int)key[i]) % Q;
            }
            return h;
        }

        public RabinKarp(string pat)
        {
            M = pat.Length;
            Rm = 1;
            for (int i = 1; i <= M-1; i++)
            {
                Rm = (R * Rm) % Q;
            }
            PatternHash = HornersHash(pat, pat.Length);
        }

        public int Search(string text)
        {
            int n = text.Length;
            long txtHash = HornersHash(text, M);
            if (PatternHash == txtHash)
            {
                return 0;
            }
            for (int i = M; i < n; i++)
            {
                //  Q is used to keep a positive value on the RHS of txtHash 
                txtHash = (txtHash + (Q - Rm * text.ElementAt(i - M) % Q)) % Q;
                txtHash = (txtHash * R + text.ElementAt(i)) % Q;
                if (PatternHash ==  txtHash)
                {
                    return i - M + 1; 
                }
            }
            return n;
        }
        
    }
}
