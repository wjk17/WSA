using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.BT
{
    public delegate int Execute();
    [Serializable]
    public class Node
    {
        public Agent agent;
        public Execute execute;
        public List<Node> childs;
        public Node() { childs = new List<Node>(); }
        public Node(Execute execute)
        {
            this.execute = execute;
        }
        public int IndexOf(Node node)
        {
            var idx = childs.IndexOf(node);
            if (idx > -1) return idx;
            foreach (var child in childs)
            {
                idx = IndexOf(node);
                if (idx > -1) return idx;
            }
            return -1;
        }
        public void Add(Node child)
        {
            childs.Add(child);
        }
        public void Add(PreCondition preCond, Action child)
        {
            child.preCond = preCond;
            childs.Add(child);
        }
        public virtual int Execute()
        {
            agent.last = this;
            return execute != null ? execute() : 1;
        }
    }
}
