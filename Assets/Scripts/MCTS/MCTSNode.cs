using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ_MCTS
{
    public class MCTSNode<T>
    {
        public MCTSNode<T> parent = null;
        public T data;
        public List<MCTSNode<T>> list = new List<MCTSNode<T>>();
        public int[] action = new int[2];
        public float wi;
        public float ni;

        public void Clear()
        {
            ni = 0;
            wi = 0;
            action[0] = 0;
            action[1] = 0;
            data = default(T);
            list.Clear();
        }

        public MCTSNode<T> Add(MCTSNode<T> t)
        {
            list.Add(t);
            t.parent = this;
            return t;
        }
    }
}