using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.BT
{
    public class Agent
    {
        public Node root;
        public Node running;
        public Node last;
        public int prevState = 0;
        public bool debug;
        public void RunBT()
        {
            if (prevState == 2)
            {
                prevState = running.Execute(); // 无视前提条件
                if (debug) Debug.Log("running");
            }
            else
            {
                prevState = root.Execute();
                if (debug) Debug.Log("root");
            }
        }
        public Agent(Node root)
        {
            this.root = root;
            SetAgent(root, this);
        }
        public void SetAgent(Node node, Agent agent)
        {
            node.agent = agent;
            foreach (var child in node.childs)
            {
                SetAgent(child, agent);
            }
        }

        internal int IndexOf(Node last)
        {
            return root.IndexOf(last);
            
        }
    }
}