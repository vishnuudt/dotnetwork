using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class B_Trees<K, V> : LLRBTree<K , V> where K : IComparable
    {
        // mostly same as LLRB but with splits and merges (2-3-4 trees). There are variants like B+, B*, B# trees. 
    }
}
