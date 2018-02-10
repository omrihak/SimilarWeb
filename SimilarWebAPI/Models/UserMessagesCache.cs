using System;
using System.Collections.Generic;

namespace SimilarWebAPI.Models
{
    public class MyCache<T> where T : ICachable
    {
        private Stack<T> stack = new Stack<T>();

        public DateTime GetLatestDateTime()
        {
            if(stack.Count == 0)
            {
                return new DateTime(1800, 1, 1);
            } else
            {
                return stack.Peek().GetDateTime();
            }
        }

        public T[] GetData()
        {
            return stack.ToArray();
        }

        public void AddObjects(List<T> newObjects)
        {
            foreach(T obj in newObjects)
            {
                stack.Push(obj);
            }
        }

        public void AddObject(T obj)
        {
            stack.Push(obj);
        }

        public void RemoveMessage()
        {
            stack.Pop();
        }
    }
}
