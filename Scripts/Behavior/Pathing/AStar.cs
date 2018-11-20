using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.Pathing
{
    public class AStar : MonoBehaviour
    {
        class Node
        {
            public List<Node> childs;
            public float f;
        }
        List<Node> OPEN;
        List<Node> CLOSE;
        void Run()
        {
            Node n = new Node();
            Node target = new Node();
            float _f = 0;
            while (OPEN.NotEmpty())
            {
                //从OPEN表中取f(n)最小的节点n;
                if (n == target) // n节点 == 目标节点
                    break;
                foreach (var X in n.childs) // 当前节点n的每个子节点X
                {
                    //计算f(X);
                    var f = F(X);
                    if (OPEN.Contains(X))
                    {
                        if (f < _f)
                        {
                            n.childs.Add(X); // 把n设置为X的父亲;
                            _f = f; // 更新OPEN表中的f(n);
                        }
                    }
                    else if (CLOSE.Contains(X))
                        continue;
                    else
                    {
                        n.childs.Add(X); //把n设置为X的父亲;
                        F(X); //求f(X);
                        OPEN.Add(X); //并将X插入OPEN表中;//还没有排序
                    }
                }
                CLOSE.Add(n); // 将n节点插入CLOSE表中;
                OPEN.Sort(Sort); //按照f(n)将OPEN表中的节点排序;//实际上是比较OPEN表内节点f的大小，从最小路径的节点向下进行。
            }
        }

        private float F(Node x)
        {
            throw new NotImplementedException();
        }

        int Sort(Node a, Node b)
        {
            if (a.f > b.f) { return 1; } ///顺序从低到高
            else if (a.f < b.f) { return -1; }
            return 0;
        }
    }
}