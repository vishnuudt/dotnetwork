using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    // Note: Patricia does not store one character per node. Instead stores a sequence of it.
    // Suffix trees will store the suffix table in a patricia trie. 
    // The below is a ternarySearchTrie without rotations.

    public class TernaryTrieNode<V>
    {
        public char CharKey { get; set; }

        public V Value { get; set; }

        public TernaryTrieNode<V> Left { get; set; }
        public TernaryTrieNode<V> Middle { get; set; }
        public TernaryTrieNode<V> Right { get; set; }

        public TernaryTrieNode<V> Get(char charKey)
        {
            var response = charKey.CompareTo(CharKey);
            if (response < 0)
            {
                if (Left != null)
                {
                    return Left.Get(charKey);
                }
                return null;
            }
            else if (response > 0)
            {
                if (Right != null)
                {
                    Right.Get(charKey);
                }
                return null;
            }
            else
            {
                if (Middle != null)
                {
                    Middle.Get(charKey);
                }
                return null;
            }
        }
    }

    public class TernarySearchTrie<V>
    {
        private TernaryTrieNode<V> _root;

        public void put(string key, V value)
        {
            _root = PutHelper(key, value, _root, 0);
        }

        // Delete is just to set the value to null.

        private TernaryTrieNode<V> PutHelper(string key, V value, TernaryTrieNode<V> node, int d)
        {
            char charKey = key.ElementAt(d);
            if (node == null)
            {
                node = new TernaryTrieNode<V> { CharKey = charKey };
            }

            var response = charKey.CompareTo(node.CharKey);
            if (response < 0)
            {
                node.Left = PutHelper(key, value, node.Left, d);
            }
            else if (response > 0)
            {
                node.Right = PutHelper(key, value, node.Right, d);
            }
            else if (d < key.Length - 1)
            {
                node.Middle = PutHelper(key, value, node.Right, d+1);
            }
            else
            {
                node.Value = value;
            }
            return node;
        }

        public TernaryTrieNode<V> Get(string key)
        {
            return GetHelper(key, _root, 0);
        }

        // Search miss is the beauty here.
        private TernaryTrieNode<V> GetHelper(string key, TernaryTrieNode<V> node, int d)
        {
            if (d == key.Length || node == null)
            {
                return node;
            }
            char c = key.ElementAt(d);
            var parent = node.Get(c);
            return GetHelper(key, parent, d + 1);
        }

        public IEnumerable<string> Keys()
        {
            Queue<string> output = new Queue<string>();
            StringBuilder strB = new StringBuilder();
            TraverseTrie(_root, output, strB);
            return output;
        }

        public IEnumerable<string> KeysWithPrefix(string s)
        {
            Queue<string> output = new Queue<string>();
            StringBuilder strB = new StringBuilder();
            var startingNode = Get(s);
            TraverseTrie(startingNode, output, strB);
            return output;
        }

        public IEnumerable<string> KeysThatMatch(string s)
        {
            // TODO: Implement this
            return Enumerable.Empty<string>();
        }

        public string LongestPrefixOf(string s)
        {
            var len = SearchTrie(s, _root, 0, 0);
            return s.Substring(0, len);
        }

        private int SearchTrie(string key, TernaryTrieNode<V> node, int d, int length)
        {
            if (node == null)
            {
                return length;
            }
            if (node.Value != null)
            {
                length = d;
            }
            if (d == key.Length)
            {
                return length;
            }
            char c = key.ElementAt(d);
            var parent = node.Get(c);
            return SearchTrie(key, parent, d + 1, length);
        }

        private void TraverseTrie(TernaryTrieNode<V> node, Queue<string> output, StringBuilder builder)
        {
            if (node == null)
            {
                builder.Clear();
                return;
            }
            builder.Append(node.CharKey);
            if (node.Value != null)
            {
                output.Enqueue(builder.ToString());
            }
            else
            {
                foreach(var child in new[] { node.Left, node.Middle, node.Right })
                {
                    TraverseTrie(child, output, builder);
                }
            }
        }
    }
}
