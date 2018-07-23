using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class PermutationAndCombination
    {
        public static void PrintCombinations(char[] arr)
        {
            int[] aux = new int[26];
            int k = arr.Length;
            HelpCombination(aux, 0, k);
        }

        private static void HelpCombination(int[] aux, int v, int k)
        {
            if (v == k)
            {
                aux[v] = 0; Console.WriteLine(string.Join("", aux));
                aux[v] = 1; Console.WriteLine(string.Join("", aux));
            }
            aux[v] = 0;
            HelpCombination(aux, v + 1, k);
            aux[v] = 1;
            HelpCombination(aux, v + 1, k);
        }

        public static void PrintPermutation(char[] arr)
        {
            HelpPermutation(arr, 1, arr.Length);
        }

        private static void HelpPermutation(char[] arr, int v, int k)
        {
            if (v == k)
            {
                Console.WriteLine(string.Join("", arr));
                return;
            }
            for (int i = v; i < k; i++)
            {
                // swap each element with the next element (starting with itself ending at last element)
                var t = arr[i];
                arr[i] = arr[v];
                arr[v] = t;
                HelpPermutation(arr, v+1, k);
                t = arr[i];
                arr[i] = arr[v];
                arr[v] = t;
            }
        }
    }
}
