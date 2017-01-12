using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trees;

namespace Graphs
{
    // Should be connected, acyclic and covers all vertices.
    // Cut property helps to divide the set of vertices into two sets 
    // Such that there are crosing edges with weights which is the minimum thereby forming the spaning tree. 
    public class MinimumSpanningTree<V>
    {
        private Queue<Edge<V>> _queue = new Queue<Edge<V>>();

        public MinimumSpanningTree(EdgeWeightedGraph<V> graph)
        {
            Kruskal(graph);
        }


        // Eager prim needs indexed priority queue

        // start at first vertex, find the min weight edge and add it to the tree. 
        // Now find a min weight edge that goes out from the tree to non-tree vertex and add it and so on.
        private void LazyPrim(EdgeWeightedGraph<V> graph)
        {
            var marked = new Boolean[graph.TotalVertices()];
            var _pq = new PriorityQueueHeap<Edge<V>>();

            LazyPrimVisit(graph, graph.Edges().First().Either, marked, _pq);

            while(!_pq.IsEmpty)
            {
                // This should be extractMin (but we can subtract the weight from 100 or something 
                // such that the min weight becomes the max item
                var item = _pq.Extract();
                V start = item.Either;
                V end = item.Other;
                var startHash = ModedHash(start.GetHashCode());
                var endHash = ModedHash(end.GetHashCode());

                if (marked[startHash] && marked[endHash])
                {
                    continue;
                }

                _queue.Enqueue(item);
                if (!marked[startHash])
                {
                    LazyPrimVisit(graph, start, marked, _pq);
                }

                if (!marked[endHash])
                {
                    LazyPrimVisit(graph, end, marked, _pq);
                }
            }
        }

        private void LazyPrimVisit(EdgeWeightedGraph<V> graph, V either, Boolean[] marked, PriorityQueueHeap<Edge<V>> pq)
        {
            marked[ModedHash(either.GetHashCode())] = true;
            foreach (var edge in graph.Adjacency(either))
            {
                if (!marked[ModedHash(edge.GetHashCode())])
                {
                    pq.Insert(edge);
                }
            }
        }

        // Kruskal's algorithm is (get the min weight edge from PQ and keep adding to tree if it does not cause a cycle
        // repeat until v-1 edges are added to the tree or until all edges are exhausted.
        private void Kruskal(EdgeWeightedGraph<V> graph)
        {
            PriorityQueueHeap<Edge<V>> _pq = new PriorityQueueHeap<Edge<V>>();
            foreach (Edge<V> edge in graph.Edges())
            {
                _pq.Insert(edge);
            }
            DisjointSets_UnionFind uf = new DisjointSets_UnionFind();

            // This should be extractMin (but we can subtract the weight from 100 or something 
            // such that the min weight becomes the max item
            while (!_pq.IsEmpty && _queue.Count < graph.TotalVertices() - 1)
            {
                var item = _pq.Extract();
                V start = item.Either;
                V end = item.Other;
                var startHash = ModedHash(start.GetHashCode());
                var endHash = ModedHash(end.GetHashCode());

                // Union find works in iterated logarithm since path compression helps move the children
                // near the root or parent of the tree.
                if (!uf.Connected(startHash, endHash))
                {
                    uf.MakeSet(startHash);
                    uf.MakeSet(endHash);
                    uf.Union(startHash, endHash);
                    _queue.Enqueue(item);
                }
            }
        }

        public IEnumerable<Edge<V>> Edges()
        {
            return _queue;            
        }

        public Double Weight()
        {
            return _queue.Select(item => item.Weight).Sum();
        }

        private int ModedHash(int hashcode, int mod = 100)
        {
            return hashcode % mod;
        }
    }
}
