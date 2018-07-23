using System;
using System.Linq;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            f(new int[]{-2, -1, 0});
        }

        public static bool f(int[] arr) 
        {
            int n = arr.Length;
            int index = 0;  // starting index, the value does not matter if there is indeed a complete cycle
            for(int i = 0; i < n; i++) {  // at most n steps
                // in Java, -b < a % b < b but 0 < (a % b + b) % b < b
                index = ((index + arr[index]) % n);
                // index = ((arr.Sum()) %n + n) % n;
                if(index == 0 && i < n - 1) {  // subcyle
                     return false;
                }
            }
            return index == 0;  // are we back to the original cell after n steps
        }
    }
}
