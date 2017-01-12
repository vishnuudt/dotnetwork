using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class LLRBTree<K, V> : BST<K, V> where K : IComparable
    {
        public Boolean IsRed(BSTNode node)
        {
            return node?.Color == NodeColor.Red;
        }

        public override BSTNode RotateLeft(BSTNode node)
        {
            if(!IsRed(node.Right))
            {
                throw new ArgumentException();
            }
            var right = base.RotateLeft(node);
            right.Color = node.Color;
            node.Color = NodeColor.Red;
            return right;
        }

        public override BSTNode RotateRight(BSTNode node)
        {
            if (!IsRed(node.Left))
            {
                throw new ArgumentException();
            }
            var right = base.RotateRight(node);
            right.Color = node.Color;
            node.Color = NodeColor.Red;
            return right;
        }

        public void FlipColors(BSTNode node)
        {
            if (IsRed(node))
            {
                throw new ArgumentException();
            }
            if (!IsRed(node.Left))
            {
                throw new ArgumentException();
            }
            if (!IsRed(node.Right))
            {
                throw new ArgumentException();
            }
            node.Color = NodeColor.Red;
            node.Left.Color = NodeColor.Black;
            node.Right.Color = NodeColor.Black;
        }

        public override BSTNode Insert(K newKey)
        {
            // case where leaning right, then rotate right at bottom
            // case when two red links in apath, then rotate at top
            // leaning left and no two red links in path is the invariant.

            var node = base.Insert(newKey); // this create inner new nodes in RED

            while (node.Parent != null)
            {
                if (IsRed(node.Right) && !IsRed(node.Left))
                {
                    node = RotateLeft(node);
                }
                if (IsRed(node.Left) && IsRed(node.Parent))
                {
                    node = RotateRight(node);
                }
                if (IsRed(node.Left) && IsRed(node.Right))
                {
                    FlipColors(node);
                }
                node = node.Parent;
            }
            return node;
        }
    }
}
