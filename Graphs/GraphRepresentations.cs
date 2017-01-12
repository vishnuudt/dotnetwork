using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    public class Edge<V> : IComparable
    {

        public Edge(V v, V w, Double weight)
        {
            Either = v;
            Other = w;
            Weight = weight;
        }

        public V Either { get; set; }

        public V Other { get; set; }

        public Double Weight { get; set; }

        public int CompareTo(object obj)
        {
            var otherEdge = obj.GetType() as Edge<V>;
            if (otherEdge == null)
            {
                throw new ArgumentException();
            }
            if (otherEdge.Weight > Weight)
            {
                return -1;
            }else if  (otherEdge.Weight < Weight)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class DirectedWeightedEdge<V> : IComparable
    {

        public DirectedWeightedEdge(V v, V w, Double weight)
        {
            From = v;
            To = w;
            Weight = weight;
        }

        public V From { get; set; }

        public V To { get; set; }

        public Double Weight { get; set; }

        public int CompareTo(object obj)
        {
            var otherEdge = obj.GetType() as Edge<V>;
            if (otherEdge == null)
            {
                throw new ArgumentException();
            }
            if (otherEdge.Weight > Weight)
            {
                return -1;
            }
            else if (otherEdge.Weight < Weight)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class DirectedWeightedGraph<V> where V : IComparable
    {
        private Dictionary<string, List<DirectedWeightedEdge<V>>> _adjacencyList = 
            new Dictionary<string, List<DirectedWeightedEdge<V>>>();

        public DirectedWeightedGraph(V vertex)
        {
            _adjacencyList[vertex.GetHashCode().ToString()] = new List<DirectedWeightedEdge<V>>();
        }

        public virtual void AddEdge(DirectedWeightedEdge<V> edge)
        {
            V start = edge.From;
            V end = edge.To;
            List<DirectedWeightedEdge<V>> edgeList = null;
            if (!_adjacencyList.TryGetValue(start.GetHashCode().ToString(), out edgeList))
            {
                _adjacencyList[start.GetHashCode().ToString()] = new List<DirectedWeightedEdge<V>>();
            }
            _adjacencyList[start.GetHashCode().ToString()].Add(edge);
        }

        public IEnumerable<DirectedWeightedEdge<V>> Adjacency(V v)
        {
            return _adjacencyList[v.GetHashCode().ToString()].ToImmutableList();
        }

        public IEnumerable<DirectedWeightedEdge<V>> Edges()
        {
            return _adjacencyList.Values.SelectMany(item => item);
        }

        public int TotalVertices()
        {
            return _adjacencyList.Count;
        }

        public int TotalEdges()
        {
            return _adjacencyList.Values.Select(item => item.Count).Sum();
        }
    }


    public class EdgeWeightedGraph<V>
    {
        private Dictionary<string, List<Edge<V>>> _adjacencyList = new Dictionary<string, List<Edge<V>>>();

        public EdgeWeightedGraph(V vertex)
        {
            _adjacencyList[vertex.GetHashCode().ToString()] = new List<Edge<V>>();
        }

        public void AddEdge(Edge<V> edge)
        {
            V start = edge.Either;
            V end = edge.Other;
            List<Edge<V>> edgeList = null;
            if (!_adjacencyList.TryGetValue(start.GetHashCode().ToString(), out edgeList))
            {
                _adjacencyList[start.GetHashCode().ToString()] = new List<Edge<V>>();
            }
            _adjacencyList[start.GetHashCode().ToString()].Add(edge);
            _adjacencyList[end.GetHashCode().ToString()].Add(edge);
        }

        public IEnumerable<Edge<V>> Adjacency(V v)
        {
            return _adjacencyList[v.GetHashCode().ToString()].ToImmutableList();
        }

        public IEnumerable<Edge<V>> Edges()
        {
            return _adjacencyList.Values.SelectMany(item => item);
        }

        public int TotalVertices()
        {
            return _adjacencyList.Count;
        }

        public int TotalEdges()
        {
            return _adjacencyList.Values.Select(item => item.Count).Sum();
        }
    }

    public abstract class ShortestPath<V> where V : IComparable
    {
        public ShortestPath(DirectedWeightedGraph<V> graph, V startVertex)
        {

        }

        public abstract double DistTo(V endVertex);

        public abstract double PathTo(V endVertex);

        public abstract Boolean HasPath(V endVertex);
    }
}
