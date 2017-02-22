using System.Collections.Generic;

namespace BitAura
{
    public class Queue<T>
    {
        private List<T> coll;

        public int Count
        {
            get
            {
                return coll.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return coll.Count == 0;
            }
        }

        public Queue()
        {
            coll = new List<T>();
        }

        public void Clear()
        {
            coll.Clear();
        }

        public void Push(T obj)
        {
            coll.Add(obj);
        }

        public T Pop()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            T obj = coll[0];
            coll.RemoveAt(0);
            return obj;
        }

        public T Tail()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            return coll[coll.Count - 1];
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            return coll[0];
        }

        public bool Remove(T item)
        {
            return coll.Remove(item);
        }
    }

    public class Stack<T>
    {
        private List<T> coll;

        public int Count
        {
            get
            {
                return coll.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return coll.Count == 0;
            }
        }

        public Stack()
        {
            coll = new List<T>();
        }

        public Stack(ICollection<T> coll)
        {
            this.coll = new List<T>(coll);
        }

        public bool Remove(T item)
        {
            return coll.Remove(item);
        }

        public void Clear()
        {
            coll.Clear();
        }

        public void Push(T obj)
        {
            coll.Add(obj);
        }

        public T Pop()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            T obj = coll[coll.Count - 1];
            coll.RemoveAt(coll.Count - 1);
            return obj;
        }

        public T Tail()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            return coll[0];
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                return default(T);
            }

            return coll[coll.Count - 1];
        }
    }
}