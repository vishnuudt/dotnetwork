using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class LCA<T>
    {
        public class Node
        {
            public T Value { get; set; }

            public int Level { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public Node Sibling { get; set; }

            public Node Parent { get; set; }
        }

        public Node FindLCA(Node root, Node a, Node b)
        {
            if (a.Level > b.Level)
            {
                return FindLCA(root, a.Parent, b);

            } else if (b.Level > a.Level)
            {
                return FindLCA(root, a, b.Parent);
            }
            else
            {
                return a;
            }
        }

        private void SetLevel(Node node, int level)
        {
            node.Level = level;
            SetLevel(node.Left, level + 1);
            SetLevel(node.Right, level + 1);
        }
    }

    public class LoopInList<T> where T: IEquatable<object>
    {
        public void DetectLoop(LinkedList<T> items)
        {
            int i = 0, j = 0;
            for (; ; i +=1, j+=2 )
            {
                if (items.ElementAt(i).Equals(items.ElementAt(j)))
                {
                    i = 0;
                    break;
                }
                if (i > items.Count)
                {
                    return;
                }
            }

            for (;; i = i+1, j = j + 1)
            {
                if (items.ElementAt(i).Equals(items.ElementAt(j)))
                {
                    Console.Write("Found element");
                    break;
                }
            }
        }
    } 

    public class MatrixMove
    {
        public void DiagonalMove(int[,] matrix)
        {
            int rowLen = matrix.GetUpperBound(1);
            int colLen = matrix.GetUpperBound(2);

            for (int k = 0; k < rowLen; k++)
            {
                for (int i = k, j = 0; i > 0 && j < colLen; i = i - 1, j = j + 1)
                {
                    Console.WriteLine(matrix[i, j]);
                }
            }

            for (int k = 1; k < colLen; k++)
            {
                for (int i = rowLen, j = k; i > 0 && j < colLen; i = i - 1, j = j + 1)
                {
                    Console.WriteLine(matrix[i, j]);
                }
            }
        }
    }

    public class MedianWithoutMerge
    {
        // a and b are sorted
        public void FindMedian(int[] a, int[] b)
        {
            int startA = 0, startB = 0;
            int endA = a.Length, endB = b.Length;
            MedianHelper(a, b, startA, endA, startB, endB);
        }

        private int MedianHelper(int[] a, int[] b, int startA, int endA, int startB, int endB)
        {
            if (endA - startA == 1 || endB -startB == 1)
            {
                // case where there are only two elements in each of the arrays
                return (Math.Max(a[startA], b[startB]) + Math.Min(a[endA], b[endB])) / 2;
            }

            var medianA = a[(startA + endA) / 2];
            var medianB = b[(startB + endB) / 2];

            if (medianA == medianB)
            {
                return medianB;
            }
            else if (medianA > medianB)
            {
                startA = medianA;
                endB = medianB;
            }
            else
            {
                endA = medianA;
                startB = medianB;
            }
            return MedianHelper(a, b, startA, endA, startB, endB);
        }
    }

    public class ReverseLinkedList
    {
        public class Node
        {
            public Node Next { get; set; }
        }

        public void Reverse(Node root)
        {
            var current = root;
            Node prev = null;
            Node next = null;
            while (current.Next != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
        }
    }

    public class BitToggle
    {
        [Flags]
        public enum BitFlags
        {
            one = 1,
            two = 2, 
            three = 4,
            four = 8,
            five = 16,
            six = 32,
            seven = 64
        }

        public int ToggleBits(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            int solution = 0;
            int nextSetBit = 1;

            while (n != 0)
            {
                int intermediate = (n & 1);
                if (intermediate == 0)
                {
                    solution |= nextSetBit;
                }
                n >>= 1;
                nextSetBit <<= 1;
            }
            return solution;
        }
    }

    public class NQueens
    {
        public static int[] queens = new int[4];

        public static bool PlaceQueens(int row, int number)
        {
            for (int column = 0; column < number; column++)
            {
                if (CanBePlaced(row, column))
                {
                    queens[row] = column;
                    if (row == number)
                    {
                        Console.Write(string.Join("", queens));
                        return true;
                    }
                    else
                    {
                        if (!PlaceQueens(row + 1, number))
                        {
                            queens[row] = column;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CanBePlaced(int row, int column)
        {
            for (int i = 1; i < row-1; i++)
            {
                if (queens[i] == column || Math.Abs(queens[i] - column) == Math.Abs(i - row))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
