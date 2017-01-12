using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    class SplayTrees<K, V> : AVLTree<K, V> where K : IComparable
    {
        public BSTNode Merge(BSTNode root1, BSTNode root2)
        {
            return MergeHelper(root1, root2);
        }

        public BSTNode AVLMergeWithRoot(BSTNode root1, BSTNode root2)
        {
            BSTNode result = null;
            if (Math.Abs(root1.Height - root2.Height) <= 1)
            {
                var newRoot = MergeHelper(root1, root2);
                return newRoot;
            }
            else if (root1.Height > root2.Height)
            {
                var newRoot = AVLMergeWithRoot(root1.Right, root2);
                root1.Right = newRoot;
                newRoot.Parent = root1;
                BalanceTree(root1);
                result = root1;
            }
            else if (root1.Height < root2.Height)
            {
                var newRoot = AVLMergeWithRoot(root2.Right, root1);
                root2.Right = newRoot;
                newRoot.Parent = root2;
                BalanceTree(root2);
                result = root2;
            }
            return result;
        }

        private BSTNode MergeHelper(BSTNode root1, BSTNode root2)
        {
            var tree1 = new BST<K, V>();
            tree1.Root = root1;

            var tree2 = new BST<K, V>();
            tree2.Root = root2;

            BSTNode rightNode = null;
            BSTNode parent = null;
            BST<K, V> finalRoot = null;

            if (root1.Height > root2.Height)
            {
                rightNode = root1.Right;
                while (rightNode.Right != null)
                {
                    rightNode = rightNode.Right;
                }
                parent = tree1.Remove(rightNode.Key);
                finalRoot = tree1;
            }
            else
            {
                rightNode = root2.Right;
                while (rightNode.Right != null)
                {
                    rightNode = rightNode.Right;
                }
                parent = tree2.Remove(rightNode.Key);
                finalRoot = tree2;
            }

            rightNode.Left = root1;
            rightNode.Right = root2;

            root1.Parent = rightNode;
            root2.Parent = rightNode;

            parent.Right = rightNode;
            rightNode.Parent = parent;

            BalanceTree(parent);
            return finalRoot.Root;
        }

        public Tuple<BSTNode, BSTNode> Split(BSTNode root, K splitAtKey)
        {
            if (root == null)
            {
                return Tuple.Create<BSTNode, BSTNode>(null, null);
            }
            var result = splitAtKey.CompareTo(root.Key);
            if (result < 0)
            {
                var tuple = Split(root.Left, splitAtKey);
                var combined = MergeHelper(tuple.Item2, root.Right);
                return Tuple.Create<BSTNode, BSTNode>(tuple.Item1, combined);
            }
            else if (result > 0)
            {
                var tuple = Split(root.Right, splitAtKey);
                var combined = MergeHelper(tuple.Item1, root.Left);
                return Tuple.Create<BSTNode, BSTNode>(tuple.Item2, combined);
            }
            else
            {
                return Tuple.Create<BSTNode, BSTNode>(root.Left, root.Right);
            }
        }

        public void Splay(BSTNode node)
        {
            // dynamic optimality conjecture
            // Zig-Zig case where parent and gParent are on the same side of the node
            if (node.Parent.Left == node && node.Parent.Parent.Left == node.Parent)
            {
                // swap grandparent with node and make them all right leaning 
                var gParent = node.Parent.Parent;
                var parent = node.Parent;
                RotateRight(gParent);
                RotateRight(parent);
            }
            // zig-zag case if Parent and GParent are on the opposide of the node then make the node parent of both parent and gparent.
            if (node.Parent.Right == node && node.Parent.Parent.Left == node.Parent
                || (node.Parent.Left == node && node.Parent.Parent.Right == node.Parent))
            {
                var gParent = node.Parent.Parent;
                var parent = node.Parent;
                RotateLeft(parent);
                RotateRight(gParent);
            }
            // Zig case when node is just below root
            if (node.Parent.Parent == null)
            {
                RotateLeft(node.Parent);
            }
            // Keep splaying until this hits the root of he tree
            if (node.Parent != null)
            {
                Splay(node);
            }
        }
    }
}
