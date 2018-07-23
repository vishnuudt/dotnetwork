using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    // All possible prefix of the problem data space.
    // so start with the final position of the elements in the solution space.
    public class DynamicProgramming
    {
        // Divide and Conquer divides the problem such that the solution to one part is independent
        // from the solution to the other part.

        // whereas dynamic programming is involved with identifying a small problem which can be solved
        // independently but will be used by the next problem which composes this smaller problem. 
        // therefore this is said as athe optimal su structure for the overlapping subproblems.

        // solution is mostly computed from the array filled by previous steps.
        // Generally the problem involves the prefixes of the data sequence.

        // watchout for the number of subproblems and their relations to the larger problem.
        // Mostly they are equal size subproblems and they depend on answers from the previous
        // subproblems.

        // For Ex: thief stealing from line of houses where constraints like steal from non-adjacent
        // places only. This has single dimension as the odd or even houses that are considered.
        // The recurrence for the above is either include the last item or not include it.
        // a[i] = max(a[i-1], a[i-2] + w[i]);
        public void IterateThrief(int[] a, int[] weights)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                a[i] = Math.Max(a[i - 1], a[i - 2] + weights[i]);
            }
        } // Go thru this array to find the optimal resources to steal from. 


        // For Ex: The kanpsack problem has optimal substructure using two dimensions
        // One is index of the item (which has a value and weight).
        // Two is residual capacity of the knapsack. (break down the limit into smaller units such that
        // it can be used to find best fit for the residual capacity). 
        // The recurrence for the above is based on [1,2,3...i] for items and 
        // [1,2,3,...W] for weight possibilities (which are integral)
        // a[i,w] = 0 for all i = 0 then the below method
        public void IterateKnapsack(int[,] a, int itemLen, int[] weights, int[] values, int maxResidualWeight)
        {
            for (int j = 0; j < weights.Length; j++)
            {
                a[0, j] = 0; // no item chosen for weight is zero
            }

            for (int i = 1; i < itemLen; i++)
            {
                // For each item try each weight unit (equal increments until the max limit)
                // Either exclude the current item or include it (by decreasing that much weight from the 
                // knapsack) so that its value is more overall.
                for (int w = 0; w < maxResidualWeight; w++)
                {
                    if (w < weights[i])
                    {
                        a[i, w] = Math.Max(a[i - 1, w], 0); // if the residual capacity is not enough to fit the weight, then it is 0
                    }
                    else
                    {
                        a[i, w] = Math.Max(a[i - 1, w], a[i - 1, w - weights[i]] + values[i]);
                    }

                }
            }
        }

        public void ReconstructKnapsackSolution(int[,] a, int itemLen, int maxWeight, int[] weights, int[] values)
        {
            for (int i = itemLen, w = maxWeight; i >= 0 && w >= 0;)
            {
                Console.Write(a[i, w]);
                if (a[i, w] == a[i - 1, w])
                {
                    i = i - 1;
                    continue;
                }
                else if (a[i, w] == a[i - 1, w - weights[i]] + values[i])
                {
                    i = i - 1;
                    w = w - weights[i];
                    continue;
                }
            }
        }


        // P[i, j] = min(p[i-1, j-1] + score(pattern[i], text[j]),
        //               p[i-1, j] + score(string.empty),
        //               p[i, j-1] + score(string.empty))
        // case 1: last character matches or not.
        // case 2: first string has empty last character
        // case 3: second string has empty last character
        public void IterateSequenceAlignment(string pattern, string text, Func<char, char, int> scoreFunc)
        {
            var arr = new int[text.Length, pattern.Length];
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < pattern.Length; j++)
                {
                    arr[i, j] = Math.Min(
                        Math.Min(arr[i - 1, j - 1] + scoreFunc(text[i], pattern[j]),
                                 arr[i - 1, j] + scoreFunc(text[i], ' ')),
                        arr[i, j - 1] + scoreFunc(' ', pattern[j]));

                }
            }
        }


        // Optimal binary search trees are constructured from a set of keys based on 
        // a given set of probabilities/Frequencies for each key (access pattern).

        // Cost(avg) Tree = SumOf (Probability(i). Depth Of I) for all I in the tree. i is 1 < 2 < 3 < 4 ... n.
        // The recnstuction is so that the average search time is minimal for the probabilities of access.
        // cost is C(T) = SumOfProbabilities(P[k]) for all K in Tree T + C(T1) + C(T2) where T1 and 
        // T2 are left and right subtrees respectively. 

        // we can consider it as selecting a node among 1< 2< 3 < ..n and making the left prefix as the 
        // left child and right suffix as the right child. But it is more appropriate to consider 
        // a contiguous interval from i to j so that for 1 <= i <= j <= n we can find an optimal cost to root.

        // SumOfProbabilities(P[k]) for all K between i and j is derived from the optimal substrcuture solution.
        // c[i,j] = min (SumOfProbabilities(P[k]) for all K between i and j 
        //              + c[i, r-1] 
        //              + c[r+1, j]) for r between i and j for all i and j
        public void IterateOptimalBST(int[,] cost, int[] probabilities)
        {
            var n = probabilities.Length;
            for (var s = 0; s <= n - 1; s++) // s is j-i
            {
                for (var i = 1; i <= n; i++) // i + s = j
                {
                    var probabilitySum = 0;
                    for (var p = i; p < i + s; p++)
                    {
                        probabilitySum += probabilities[p];
                    }
                    for (var r = i; r < i + s; r++)
                    {
                        cost[i, i + s] = Math.Min(
                                                Math.Min(probabilitySum, cost[i, r - 1]),
                                                cost[r + 1, i + s]);
                    }
                }
            }
            var max = cost[1, n]; // this is the probability of the root.
        }

        // Longest common subsequence is based on the matching two string sequences
        // where the first character from string one is matched against the first character of string two,
        // then with first two characters of string two, then first three character of string two
        // and so on. So it is a brute force except that intermediate results are stored.
        public enum Pointer
        {
            DiagonalLeft,
            Left,
            Up
        }

        public void LCSUsingDP(string text, string pattern)
        {
            var str1 = text.ToCharArray();
            var str2 = pattern.ToCharArray();

            var matches = new int[str1.Length, str2.Length];
            var markers = new Pointer[str1.Length, str2.Length];

            for (int i = 0, j = 0; i < str1.Length && j < str2.Length; ++i, ++j)
            {
                matches[i, 0] = 1;
                matches[0, j] = 1;
            }

            // recurrence relation
            // if str1[i] == str2[j] matches[i, j] = 1 + (matches[i-1, j-1])
            // else matches[i, j] = Min(matches[i-1, j], matches[i, j-1])
            for (int i = 0; i < str1.Length; i++)
            {
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j])
                    {
                        if (i == 0 || j == 0)
                        {
                            matches[i, j] = 1;
                            continue;
                        }
                        markers[i, j] = Pointer.DiagonalLeft;
                        matches[i, j] = 1 + matches[i - 1, j - 1];
                    }
                    else
                    {
                        if (matches[i - 1, j] > matches[i, j - 1] || (j == 0 && i > 0))
                        {
                            matches[i, j] = matches[i - 1, j];
                            markers[i, j] = Pointer.Left;
                        }
                        else
                        {
                            matches[i, j] = matches[i, j - 1];
                            markers[i, j] = Pointer.Up;
                        }
                    }
                }
            }
        }

        // palindrome recurrence not well established except that palindrome is 
        // created at each substring length and a sliding window of such lengths.
        public void PalindromeSubstring(string text)
        {
            var str1 = text.ToCharArray();

            int pal_length = 0;
            int pal_beginsAt = 0;
            var matches = new bool[str1.Length, str1.Length];

            // for 1 character matches
            for (int i = 0; i < str1.Length; i++)
            {
                matches[i, i] = true;
            }

            // for 2 character matches
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] == str1[i+1])
                {
                    pal_length = 2;
                    pal_beginsAt = i;
                    matches[i, i + 1] = true;
                }
            }

            // for 3 to n characters
            for (int curr_length = 3; curr_length < str1.Length; curr_length++)
            {
                for (int j = 0; j < str1.Length - curr_length + 1; j++)
                {
                    int k = j + curr_length - 1;
                    if (str1[j] == str1[k] && matches[j+1, k-1])
                    {
                        pal_beginsAt = j;
                        pal_length = curr_length;
                        matches[j, k] = true;
                    }
                }
            }
        }
    }
}
