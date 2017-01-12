using System;

namespace Trees
{
    public class K_DTrees_Geometric_<K, V> : BST<K, V> where K : IComparable
    {

        // for 1 dimensional keys then search left, compare root, then search right for the range. 
        
        // for line intersection, we could use sweep line algorithm ( iterate thru x in sorted order andinsert the y into BST (thereby making it a 1d range search)
        // if the same y is seen again then remove it, if there is another x and y seen then do a 1d range search to find the intersection)

        // for 2 dimensional keys use a tree to divide the space in geometry
        // for example divide plane by first dimension then divide by second dimension, then again by first then so on...
        public class KDNode : BSTNode
        {
            public KDDimensionType Dimension { get; set; }
        }

        public new KDNode Root { get; set; }

        public enum KDDimensionType
        {
            Horizontal,
            Vertical
        }

        public KDNode Insert(Tuple<K, K> newKey)
        {
            var parentNode = Find(newKey.Item1);
            // alternate between x and y as we insert also with dimension
            var result = base.Insert(newKey.Item1);
            return null;
        }


        public KDNode NearestNeighbour(Tuple<K, K> newKey)
        {
            // use formula to calculate distance between two points.
            return null;
        }
    }
}
