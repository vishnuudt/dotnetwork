using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public class AVLTree<K, V> : BST<K, V> where K : IComparable
    {
        public override BSTNode Insert(K newKey)
        {
            var value = base.Insert(newKey);
            BalanceTree(value);
            return value;
        }

        public override BSTNode Remove(K newKey)
        {
            var parent = base.Remove(newKey);
            BalanceTree(parent);
            return parent;
        }

        public void BalanceTree(BSTNode node)
        {
            var parent = node.Parent;
            if (node.Left.Height > node.Right.Height + 1)
            {
                RebalanceRight(node);
            }
            else if (node.Right.Height > node.Left.Height + 1)
            {
                RebalanceLeft(node);
            }
            if (parent != null)
            {
                BalanceTree(parent);
            }
        }

        public void RebalanceRight(BSTNode node)
        {
            var holdLeft = node.Left;
            if (holdLeft.Right.Height > holdLeft.Left.Height)
            {
                RotateLeft(holdLeft);
            }
            RotateRight(node);
        }

        public void RebalanceLeft(BSTNode node)
        {
            RotateLeft(node);
        }
    }
}
