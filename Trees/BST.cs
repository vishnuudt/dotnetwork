using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trees
{
    public class BST<K, V> where K : IComparable
    {
        public enum NodeColor
        {
            Red,
            Black
        }

        public class BSTNode
        {
            public BSTNode Parent { get; set; }
            public BSTNode Left { get; set; }
            public BSTNode Right { get; set; }
            public V Value { get; set; }
            public K Key { get; set; }

            public NodeColor Color { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public int Height
            {
                get
                {
                    var leftH = Left?.Height;
                    var rightH = Right?.Height;
                    return 1 + Math.Max(leftH != null ? 0 : (int)leftH, rightH != null ? 0 : (int)rightH);

                }

            }
        }

        public BSTNode Root { get; set; }

        public virtual BSTNode Find(K key)
        {
            if (Root == null)
            {
                return Root;
            }
            return FindHelper(key, Root);
        }

        protected BSTNode FindHelper(K key, BSTNode node)
        {
            int outcome = key.CompareTo(node.Key);
            if (outcome > 0)
            {
                if (node.Right == null)
                {
                    return node;
                }
                return FindHelper(key, node.Right);
            }
            else if (outcome < 0)
            {
                if (node.Left == null)
                {
                    return node;
                }
                return FindHelper(key, node.Left);
            }
            else
            {
                return node;
            }
        }

        public virtual BSTNode Next(K key)
        {
            var node = Find(key);
            return NextHelper(key, node);
        }

        protected BSTNode NextHelper(K key, BSTNode node)
        {
            // there is no successor, go to parent and check
            if (node.Right == null)
            {
                while (node.Parent != null)
                {
                    if (node.Key.CompareTo(key) > 0)
                    {
                        return node;
                    }
                    node = node.Parent;
                }
                return node;
            }

            // there is a successor, go right then go all the way to left
            node = node.Right;
            while (node.Left != null)
            {
                node = node.Left;
            }

            return node;
        }


        public virtual IEnumerable<BSTNode> Range(K startKey, K endKey)
        {
            var node = Find(startKey);
            return RangeHelper(startKey, endKey, node);
        }

        protected IEnumerable<BSTNode> RangeHelper(K startKey, K endKey, BSTNode node)
        {
            // there is no successor, go to parent and check
            IList<BSTNode> list = new List<BSTNode>();
            while (node.Key.CompareTo(endKey) <= 0)
            {
                if (node.Key.CompareTo(startKey) >= 0)
                {
                    list.Add(node);
                }
                node = Next(node.Key);
            }
            return list;
        }


        public virtual BSTNode Insert(K newKey)
        {
            var node = Find(newKey);
            if (node == null)
            {
                Root = new BSTNode { Key = newKey };
                return Root;
            }
            return InsertHelper(newKey, node);
        }

        protected BSTNode InsertHelper(K newKey, BSTNode node)
        {
            var outcome = node.Key.CompareTo(newKey);
            if (outcome > 0)
            {
                node.Left = new BSTNode { Parent = node, Key = newKey, Color = NodeColor.Red };
                return node.Left;
            }
            if (outcome < 0)
            {
                node.Right = new BSTNode { Parent = node, Key = newKey, Color = NodeColor.Red };
                return node.Right;
            }
            return node;
        }

        public virtual BSTNode Remove(K newKey)
        {
            var node = Find(newKey);
            RemoveHelper(newKey, node);
            return node.Parent;
        }

        protected void RemoveHelper(K newKey, BSTNode node)
        {
            if (node.Right == null)
            {
                node.Left.Parent = null;
                Root = node.Left;
            }
            else
            {
                var successor = Next(newKey);
                node.Left.Parent = successor;
                node.Right.Parent = successor;
                successor.Parent.Left = successor.Right;
                successor.Right.Parent = successor.Parent;
                UpdateParent(node, successor);
            }
        }

        public virtual BSTNode RotateLeft(BSTNode node)
        {
            var right = node.Right;
            node.Right = right.Left;
            right.Left = node;
            UpdateParent(node, right);
            return right;
        }

        public virtual BSTNode RotateRight(BSTNode node)
        {
            var left = node.Left;
            node.Left = left.Right;
            left.Right = node;
            UpdateParent(node, left);
            return left;
        }

        public void UpdateParent(BSTNode node, BSTNode successor)
        {
            if (node.Parent.Left == node)
            {
                node.Parent.Left = successor;
            }
            else
            {
                node.Parent.Right = successor;
            }
        }
    }
}
