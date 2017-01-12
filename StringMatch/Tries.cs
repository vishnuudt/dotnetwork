using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringMatch
{
    // Keys and Chars need not be explicitly stored.
    public class TrieNode<V>
    {

        // whole point of trie is to avoid hashing so we could use arrays with size fo radix
        // but this would be very wateful so we do TernarySearchTrie
        // if we also keep it balanced then it would guarantee big o.
        Dictionary<char, TrieNode<V>> _nextNodes = new Dictionary<char, TrieNode<V>>();
        public char CharKey { get; set; }

        public V Value { get; set; }

        public bool IsEmpty() => _nextNodes.Count == 0;

        public TrieNode<V> Add(char charKey)
        {
            TrieNode<V> output = null;
            if (!_nextNodes.TryGetValue(charKey, out output))
            {
                output = new TrieNode<V> { CharKey = charKey };
                _nextNodes[charKey] = output;
            }
            return output;
        }

        public TrieNode<V> Get(char charKey)
        {
            TrieNode<V> output = null;
            _nextNodes.TryGetValue(charKey, out output);
            return output;
        }
    }

    // Rway Tries
    public class Tries<V>
    {
        private int R = 256; // Radix (extended ascii)
        private TrieNode<V> _root = new TrieNode<V>();

        public void put(string key, V value)
        {
            PutHelper(key, value, _root, 0);
        }

        // Delete is just to set the value to null.

        private void PutHelper(string key, V value, TrieNode<V> node, int d)
        {
            if (d == key.Length)
            {
                node.Value = value;
                return;
            }
            char c = key.ElementAt(d);
            var parent = node.Add(c);
            PutHelper(key, value, parent, d + 1);
        }

        public void Get(string key)
        {
            GetHelper(key, _root, 0);
        }

        // Search miss is the beauty here.
        private TrieNode<V> GetHelper(string key, TrieNode<V> node, int d)
        {
            if (d == key.Length || node == null)
            {
                return node;
            }
            char c = key.ElementAt(d);
            var parent = node.Get(c);
            return GetHelper(key, parent, d + 1);
        }
    }
}
