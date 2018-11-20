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
        public void Add(Node child)
        {
            childs.Add(child);
        }
        public virtual int Execute()
        {
            return execute != null ? execute() : 1;
        }
    }
}
