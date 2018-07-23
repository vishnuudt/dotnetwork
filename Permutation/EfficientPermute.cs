using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permutation
{
    public class EfficientPermute
    {
        public Boolean IsOdd(int n)
        {
            return n % 2 != 0;
        }

        public Boolean IsEven(int n)
        {
            return !IsOdd(n);
        }

        // below this algo is aeasy to read algo
        public EfficientPermute(int n)
        {
            var permutedArray = new int[n];
            var inversionArray = new int[n];
            int total_inversion = 0, i = 0, c, sum_left_inversion = 0;

            while (i < n)
            {
                permutedArray[i] = i++;
            }

            inversionArray[0] = -2;
            var ready = false;
            while(!ready)
            {
                Console.WriteLine(string.Join("",permutedArray));
                i = n - 1;
                c = 0;
                sum_left_inversion = total_inversion - inversionArray[i];

                while (inversionArray[i] == i && IsEven(sum_left_inversion))
                {
                    c = c + 1;
                    i = i - 1;
                    sum_left_inversion = sum_left_inversion - inversionArray[i];
                }

                while (inversionArray[i] == 0 && IsOdd(sum_left_inversion))
                {
                    i = i - 1;
                    sum_left_inversion = sum_left_inversion - inversionArray[i];
                }

                c = c + i - inversionArray[i];

                if (IsEven(sum_left_inversion) && i > 0)
                {
                    inversionArray[i] = inversionArray[i] + 1;
                    total_inversion = total_inversion + 1;
                    var temp = permutedArray[c - 1];
                    permutedArray[c - 1] = permutedArray[c];
                    permutedArray[c] = temp;
                }
                else if (IsOdd(sum_left_inversion) && i > 0)
                {
                    inversionArray[i] = inversionArray[i] - 1;
                    total_inversion = total_inversion - 1;
                    var temp = permutedArray[c + 1];
                    permutedArray[c + 1] = permutedArray[c];
                    permutedArray[c] = temp;
                }
                else
                {
                    ready = true;
                }
            }
        }

        // Assume the whole string is distinct
        public void PermuteEase(string str)
        {
            var map = new Dictionary<char, int>();
            for (var i = 0;  i < str.Length; i++)
            {
                int value = 0;
                if (map.TryGetValue(str[i], out value))
                {
                    map[str[i]] = ++value;
                }
                else
                {
                    map[str[i]] = value;
                }
            }

            var elements = map.Keys.ToArray();
            var result = new char[elements.Count()];
            var count = map.Values.ToArray();
            HelpPermute(elements, count, result, 0);
        }

        private void HelpPermute(char[] elements, int[] count, char[] result, int level)
        {
            if (level == elements.Count())
            {
                Console.WriteLine(string.Join("", result));
                return;
            }

            // at each level of recursion we eat up one count of the character
            // this is done until all the characters are eaten up to reach all 0
            // The algo works like finding each different path to zero from some 
            // count, reaching zero means that permute is ok. 
            for (int i = 0; i < elements.Length; i++)
            {
                if (count[i] == 0)
                {
                    continue;
                }
                count[i]--;
                result[level] = elements[i];
                HelpPermute(elements, count, result, level + 1);
                count[i]++;
            }
        }
    }
}
