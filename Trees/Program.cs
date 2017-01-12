using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class Program
    {
        static void Main(string[] _)
        {
            BST<int, int> tree = new BST<int, int>();
            foreach (var item in Enumerable.Range(1 , 10))
            {
                tree.Insert(item);
            }
            Console.Write(tree.Find(5));
            Console.Read();
        }
    }
}
