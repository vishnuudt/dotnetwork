using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees
{
    public enum PriorityQueueType
    {
        Max,
        Min
    }

    public class PriorityQueueHeap<K> where K : IComparable
    {
        protected int _maxSize = Int32.MaxValue;
        protected int _size = 0;
        // 1 based indexing
        private K[] pq = new K[Int32.MaxValue];
        protected PriorityQueueType _queueType = PriorityQueueType.Min;

        public PriorityQueueHeap()
        {
            _queueType = PriorityQueueType.Min;
        }

        public PriorityQueueHeap(PriorityQueueType queueType)
        {
            _queueType = queueType;
        }

        public virtual void Insert(K key)
        {
            if (_size == _maxSize)
            {
                throw new ArgumentOutOfRangeException();
            }
            _size += 1;
            pq[_size] = key;
            SiftUp(_size);
        }

        public Boolean IsEmpty => _size == 0;

        public virtual K Extract()
        {
            // take the top most, move the bottom most to the top and then sift the new top towards downward
            // to find the correct position. 
            var result = pq[1];
            pq[1] = pq[_size];
            _size -= 1;
            SiftDown(1);
            return result;
        }

        public void SiftUp(int i)
        {
            // If i is not the parent and its parent is less than itself, keep swapping (swim up)
            while (i > 1 && CompareForQueueType(ParentIndex(i), i))
            {
                var temp = pq[i];
                pq[i] = pq[ParentIndex(i)];
                pq[ParentIndex(i)] = temp;
                i = ParentIndex(i);
            }
        }

        public void SiftDown(int parentIndex)
        {
            int maxChildIndex = parentIndex;
            int leftIndex = LeftChild(parentIndex);
            int rightIndex = RightChild(parentIndex);

            if (leftIndex < _size && CompareForQueueType(maxChildIndex, leftIndex))
            {
                maxChildIndex = leftIndex;
            }

            if (rightIndex < _size && CompareForQueueType(maxChildIndex, rightIndex))
            {
                maxChildIndex = rightIndex;
            }
            if (parentIndex != maxChildIndex)
            {
                Exchange(parentIndex, maxChildIndex);
                SiftDown(maxChildIndex);
            }
        }

        protected virtual void Exchange(int parentIndex, int maxChildIndex)
        {
            var temp = pq[parentIndex];
            pq[parentIndex] = pq[maxChildIndex];
            pq[maxChildIndex] = temp;
        }

        protected virtual bool CompareForQueueType(int parentIndex, int childIndex)
        {
            return _queueType == PriorityQueueType.Max
                ? pq[parentIndex].CompareTo(pq[childIndex]) < 0
                : pq[parentIndex].CompareTo(pq[childIndex]) > 0;
        }

        public int ParentIndex(int i)
        {
            var doubleValue = Math.Floor(i / 2.0);
            return Convert.ToInt32(doubleValue);
        }

        public int LeftChild(int i)
        {
            return 2 * i;
        }

        public int RightChild(int i)
        {
            return (2 * i) + 1;
        }
    }

    public class IndexedPriorityQueueHeap<K> : PriorityQueueHeap<K> where  K : IComparable
    {
        private int[] pq = new int[Int32.MaxValue];
        private int[] qp = new int[Int32.MaxValue];
        private K[] keys = new K[Int32.MaxValue];

        public IndexedPriorityQueueHeap()
        {
            for (int i = 0; i < Int32.MaxValue; ++i)
            {
                qp[i] = -1;
            }
        }

        public IndexedPriorityQueueHeap(PriorityQueueType queueType) : base(queueType)
        {

        }

        public void Insert(int i, K key)
        {
            _size += 1;
            // store the ordinal for index i
            qp[i] = _size;
            // store the index i for ordinal
            pq[_size] = i;
            // store the key for index i
            keys[i] = key;
            SiftUp(_size);
        }

        public override K Extract()
        {
            var min = pq[1];
            pq[1] = pq[_size];
            _size -= 1;
            SiftDown(1);

            qp[min] = -1;
            var result = keys[min];
            keys[min] = default(K);
            pq[_size + 1] = -1;

            return result;
        }

        protected override bool CompareForQueueType(int parentIndex, int childIndex)
        {
            return _queueType == PriorityQueueType.Max
                ? keys[pq[parentIndex]].CompareTo(keys[pq[childIndex]]) < 0
                : keys[pq[parentIndex]].CompareTo(keys[pq[childIndex]]) > 0;
        }

        protected override void Exchange(int parentIndex, int maxChildIndex)
        {
            base.Exchange(parentIndex, maxChildIndex);
            qp[pq[parentIndex]] = parentIndex;
            qp[pq[maxChildIndex]] = maxChildIndex;
        }

        public void DecreaseKey(int i, K key)
        {
            keys[i] = key;
            SiftDown(qp[i]);
        }

        public Boolean Contains(int i)
        {
            return qp[i] != -1;
        }
    }
}
