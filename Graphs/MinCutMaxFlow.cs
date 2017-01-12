using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs
{
    // there is a source and target and the edges take a weight.
    // If the forward edges are not full and the backward edges are not empty then it is mincut vertices. 
    // Else there is no augmentation and therefore there is a maxflow. 
    public class FlowWeightedEdge<V> : DirectedWeightedEdge<V> where V : IComparable
    {
        public Double Flow { get; set; }
        public Double Capacity { get; set; }

        public V Other(V either) { return From.CompareTo(either) == 0 ? To : From; }

        public Double ResidualCapacityToVertex(V v)
        {
            if (v.CompareTo(From) == 0)
            {
                return Flow; //This is backward edge
            }
            else if (v.CompareTo(To) == 0)
            {
                return Capacity - Flow; // This is forward edge
            }
            throw new ArgumentException();
        }

        // A -> 7/9 -> B (original network) === A -> 2 -> B -> 7 -> A (residual network)
        // A -> Flow/Capacity ->  B 
        public void AddResidualFlowToVertex(V v, Double delta)
        {
            if (v.CompareTo(From) == 0)
            {
                Flow -= delta;
            }
            else if (v.CompareTo(To) == 0)
            {
                Flow += delta;
            }
            throw new ArgumentException();
        }

        public FlowWeightedEdge(V v, V w, double weight) : base(v, w, weight)
        {
            Capacity = weight;
        }
    }

    // Residual network of edges
    public class FlowWeightedGraph<V> where V : IComparable
    {
        private Dictionary<string, List<FlowWeightedEdge<V>>> _adjacencyList =
        new Dictionary<string, List<FlowWeightedEdge<V>>>();

        public FlowWeightedGraph(V vertex)
        {
            _adjacencyList[vertex.GetHashCode().ToString()] = new List<FlowWeightedEdge<V>>();
        }

        public virtual void AddEdge(FlowWeightedEdge<V> edge)
        {
            V start = edge.From;
            V end = edge.To;
            List<FlowWeightedEdge<V>> edgeList = null;
            if (!_adjacencyList.TryGetValue(start.GetHashCode().ToString(), out edgeList))
            {
                _adjacencyList[start.GetHashCode().ToString()] = new List<FlowWeightedEdge<V>>();
            }
            if (!_adjacencyList.TryGetValue(end.GetHashCode().ToString(), out edgeList))
            {
                _adjacencyList[end.GetHashCode().ToString()] = new List<FlowWeightedEdge<V>>();
            }
            _adjacencyList[start.GetHashCode().ToString()].Add(edge);
            _adjacencyList[end.GetHashCode().ToString()].Add(edge);
        }

        public IEnumerable<FlowWeightedEdge<V>> Adjacency(V v)
        {
            return _adjacencyList[v.GetHashCode().ToString()].ToImmutableList();
        }

        public IEnumerable<FlowWeightedEdge<V>> Edges()
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

    // Latest is push-relabel mathod for maximum flow
    public class MinCutMaxFlow<V> where V : IComparable
    {
        private Boolean[] marked = new bool[Int32.MaxValue];
        private FlowWeightedEdge<V>[] edgeTo = new FlowWeightedEdge<V>[Int32.MaxValue];
        private double value;

        public void FordFulkerson(FlowWeightedGraph<V> graph, V startVertex, V endVertex)
        {
            value = 0.0;
            // visit the graph and find a path from s to t, fill edgeto and marked arrays
            // this terminates when there is no augmenting path ( all full forward edges and empty backward edges)
            while (HasAugmentingPath(graph,  startVertex, endVertex))
            {
                double bottle = Double.PositiveInfinity;
                V begin = endVertex;
                for (; begin.CompareTo(startVertex) != 0; begin = edgeTo[begin.GetHashCode()].Other(begin))
                {
                    // minimum of the unused capacity in forward edge or available flow in backward edge
                    bottle = Math.Min(bottle, edgeTo[begin.GetHashCode()].ResidualCapacityToVertex(begin));
                }
                // now go thru the edge and add the residual capacity to augment the flow
                begin = endVertex;
                for (; begin.CompareTo(startVertex) != 0; begin = edgeTo[begin.GetHashCode()].Other(begin))
                {
                    edgeTo[begin.GetHashCode()].AddResidualFlowToVertex(begin, bottle);
                }
                value += bottle;
            }
        }

        public Boolean InCut(V v)
        {
            return marked[v.GetHashCode()];
        }

        // there is path from start to end in the residual network.
        private bool HasAugmentingPath(FlowWeightedGraph<V> graph, V startVertex, V endVertex)
        {
            Queue<V> queue = new Queue<V>();
            queue.Enqueue(startVertex);
            marked[startVertex.GetHashCode()] = true;
            while (queue.Count != 0)
            {
                var first = queue.Dequeue();
                foreach (var edge in graph.Adjacency(first))
                {
                    var other = edge.Other(first);
                    if (edge.ResidualCapacityToVertex(other) > 0 && !marked[other.GetHashCode()])
                    {
                        edgeTo[other.GetHashCode()] = edge;
                        marked[other.GetHashCode()] = true;
                        queue.Enqueue(other);
                    }
                }
            }
            return marked[endVertex.GetHashCode()];
        }
    }
}
