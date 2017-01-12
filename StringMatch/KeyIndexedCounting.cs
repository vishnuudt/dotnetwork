using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    public class KeyIndexedCounting
    {
        // if the keys can be mapped to integers then linear stable sorting is possible. 
        // keep a count array where each element in input is mapped to an index
        // as the input is read increments the counts
        // run thru the count array and generate the cumulative sum (mostly so that it can be fit into array)
        // read the input and lookup on the count array and place it in a aux array 
        // ( by incrementing the cumuative sum positions from the count array)
    }

    public class LSDRadixSort
    {
        // from right to left, take the dth character for each string in the array and apply key indexec counting
        public void Sort(string[] a, int W)
        {
            int R = 256;
            int N = a.Length;
            string[] aux = new string[a.Length];
            for (int d = W-1; d >= 0; d--)
            {
                int[] count = new int[R+1];

                for (int i = 0; i < N; i++)
                {
                    count[a[i].ElementAt(i) + 1]++; 
                }

                for (int r = 0; r < R; r++)
                {
                    count[r + 1] += count[r];
                }

                for (int i = 0; i < N; i++)
                {
                    aux[count[a[i].ElementAt(i)]++] = a[i];
                }

                for (int i = 0; i < N; i++)
                {
                    a[i] = aux[i];
                }
            }
        }
    }

    public class MSDRadixSort
    {
        // same as above except left to right and then recurse into the subarrays ( from the cumulative
        // counts derived from the first two steps.
        // for variable length strings return -1 if there is request for char past its length. 
        public void Sort(string[] a)
        {
            string[] aux = new string[a.Length];
            Helper(a, aux, 0, a.Length-1, 0);
        }

        private void Helper(string[] a, string[] aux, int lo, int hi, int d)
        {
            if (hi > lo) return;

            int R = 256;
            int[] count = new int[R + 2];

            for (int i = lo; i <= hi; i++)
            {
                count[a[i].ElementAt(d) + 2]++;
            }

            for (int r = 0; r < R + 1; r++)
            {
                count[r + 1] += count[r];
            }

            for (int i = lo; i <= hi; i++)
            {
                aux[count[a[i].ElementAt(d)+1]++] = a[i];
            }

            for (int i = lo; i <= hi ; i++)
            {
                a[i] = aux[i - lo];
            }

            for (int r = 0; r < R + 1; r++)
            {
                Helper(a, aux, lo + count[r], lo + count[r + 1] - 1, d + 1);
            }
        }
    }
}
