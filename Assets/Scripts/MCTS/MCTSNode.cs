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
        public int visits = 0;
        public int[] action = new int[2];
        public float wi;
        public float ni;

        public MCTSNode(T d)
        {
            this.data = d.FullCopy<T>();
        }

        public T Data
        {
            get {return data;}
        }

        public MCTSNode<T> Add(MCTSNode<T> t)
        {
            list.Add(t);
            t.parent = this;
            return t;
        }
    }
}