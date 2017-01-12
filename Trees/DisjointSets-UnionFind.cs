using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class DisjointSets_UnionFind
    {
        // This is naive array implementation O(n)
        private int[] _smallest = new int[Int32.MaxValue];

        // This is real as it stores the parent
        private int[] _parent = new int[Int32.MaxValue];

        // inorder for the union to put the smaller tree under the larger, we need a rank array. This way the tree will remain shallow.
        private int[] _rank = new int[Int32.MaxValue];

        public void Preprocess()
        {

        }

        public void MakeSet(int i)
        {
            // _smallest[i] = i;
            if (_parent[i] != default(int))
            {
                _parent[i] = i;
            }
        }

        public bool Connected(int i, int j)
        {
            var p_i = Find(i);
            var p_j = Find(j);
            return p_i == p_j;
        }

        public int Find(int i)
        {
            // return _smallest[i];

            // This is the uncompressed implementation
            //while (_parent[i] != i)
            //{
            //    i = _parent[i];
            //}
            // This is the compressed path (where each index (object) remembers its parent
            if (i != _parent[i])
            {
                _parent[i] = Find(_parent[i]);
            }
            return _parent[i];
        }

        public void Union(int i, int j)
        {
            var i_id = Find(i);
            var j_id = Find(j);
            if (i_id == j_id)
            {
                return;
            }
            if (_rank[i_id] > _rank[j_id])
            {
                _parent[j_id] = i_id;
            }
            else
            {
                _parent[i_id] = j_id;
                if (_rank[i_id] == _rank[j_id])
                {
                    _rank[j_id] += 1; // since it got a new child, its height incerases by 1.
                }
            }
        }

        //  a linked list can also be used for ex: 8 -> 9 -> 4 -> 6, Find(8) if found in list will return 6 (tail stores id)
        public void Union_UsingBruteForce(int i, int j)
        {
            var i_id = Find(i);
            var j_id = Find(j);
            if (i_id ==j_id)
            {
                return;
            }
            var min_id = Math.Min(i_id, j_id);
            int cnt = 0;
            foreach (var item in _smallest)
            {
                ++cnt;
                if (item == i_id || item == j_id)
                {
                    _smallest[cnt] = min_id; 
                }
            }
        }
    }
}
