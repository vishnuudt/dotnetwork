using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trees;

namespace Graphs
{
    public class DijkstraShortestPath<V> : ShortestPath<V> where V: IComparable
    {
        private DirectedWeightedEdge<V>[] _edgeTo = new DirectedWeightedEdge<V>[Int32.MaxValue];
        private double[] _distanceTo = new double[Int32.MaxValue];
        private IndexedPriorityQueueHeap<Double> _indexedPQ = new IndexedPriorityQueueHeap<Double>();

        public DijkstraShortestPath(DirectedWeightedGraph<V> graph,  V startVertex): base(graph, startVertex)
        {
            for (int i = 0; i < graph.TotalVertices(); i++)
            {
                _distanceTo[i] = Double.PositiveInfinity;
            }
            _distanceTo[startVertex.GetHashCode()] = 0.0;

            _indexedPQ.Insert(startVertex.GetHashCode(), 0.0);
            while (!_indexedPQ.IsEmpty)
            {
                var result = _indexedPQ.Extract(); // Min
                foreach (var edge in graph.Adjacency(result))
                {
                    Relax(edge);
                }
            }
        }

        private void Relax(DirectedWeightedEdge<V> edge)
        {
            var start = edge.From;
            var end = edge.To;
            if (_distanceTo[end.GetHashCode()] > _distanceTo[start.GetHashCode()] + edge.Weight)
            {
                _distanceTo[end.GetHashCode()] = _distanceTo[start.GetHashCode()] + edge.Weight;
                _edgeTo[end.GetHashCode()] = edge;
                // TODO: Update end with the _distanceTo[end.GetHashCode()] 
                // so that it can used in the PQ (it is the priority of the vertex that is used for swim or sink)
                if (_indexedPQ.Contains(end.GetHashCode()))
                {
                    _indexedPQ.DecreaseKey(end.GetHashCode(), _distanceTo[end.GetHashCode()]);
                }
                else
                {
                    _indexedPQ.Insert(end.GetHashCode(), _distanceTo[end.GetHashCode()]);
                }
            }
        }

        public override double DistTo(V endVertex)
        {
            throw new NotImplementedException();
        }

        public override bool HasPath(V endVertex)
        {
            throw new NotImplementedException();
        }

        public override double PathTo(V endVertex)
        {
            throw new NotImplementedException();
        }
    }
}
