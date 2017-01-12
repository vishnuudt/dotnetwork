using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    public class SuffixArrays
    {
        // Longest repeated substring
        // menber myers uses similar algo but does MSD on 1 digit, then 2 digits together, then 4 , then 8 so on...
        public string LRS(string s)
        {
            string[] suffixes = new string[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                suffixes[i] = s.Substring(i, s.Length);
            }
            Array.Sort(suffixes);
            string lrs = "";
            for (int i = 0; i < s.Length -1; i++)
            {
                int len = LCP(suffixes[i], suffixes[i + 1]);
                if (len > lrs.Length)
                {
                    lrs = suffixes[i].Substring(0, len);
                }
            }
            return lrs;
        }

        private int LCP(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
