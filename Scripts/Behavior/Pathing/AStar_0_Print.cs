using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.Pathing
{
    public partial class AStar_0
    {
        public enum NodeType
        {
            Reachable = 0, //可以到达的结点
            Bar = 1, //障碍物
            Pass = 2, //需要走的步数
            Source = 3, //起点
            Destination = 4 //终点
        }
        public string[] Symbol = new string[] { "□", "■", "▽", "○", "☆", }; //◎
        public int padLen = 5;
        public string split = ",";
        public int fracCount = 1;

        void PrintMap(string str)
        {
            str += ":\r\n";
            for (int i = 0; i < size.y; i++)
            {
                string l = "";
                for (int j = 0; j < size.x; j++)
                {
                    l += Symbol[(int)graph[i, j].value];
                }
                str += l + "\r\n";
            }
            Debug.Log(str);
        }
        string GetDir(int flag)
        {
            //return string.Format("{0:X}", flag);
            var dirStr = new string[] { "e", "se", "s", "sw", "w", "nw", "n", "ne" };
            string str = "";
            for (int i = 0; i < 8; i++)
            {
                if ((flag & (1 << i)) > 0)
                {
                    str += i + 1;// dirStr[i];// + ",";
                }
            }
            if (str.Empty()) str = Symbol[1];
            return str;
        }
        //const int East = (1 << 0);
        //const int South_East = (1 << 1);
        //const int South = (1 << 2);
        //const int South_West = (1 << 3);
        //const int West = (1 << 4);
        //const int North_West = (1 << 5);
        //const int North = (1 << 6);
        //const int North_East = (1 << 7);
        //new Vector2Int(0, 1),   // East
        //new Vector2Int(1, 1),   // South_East
        //new Vector2Int(1, 0),   // South
        //new Vector2Int(1, -1),  // South_West
        //new Vector2Int(0, -1),  // West
        //new Vector2Int(-1, -1), // North_West
        //new Vector2Int(-1, 0),  // North
        //new Vector2Int(-1, 1)   // North_East
        //public string[] dirStr = new string[] { "E", "SE", "S", "SW", "W", "NW", "N", "NE" };
        //public string[] dirStr = new string[] { "e", "se", "s", "sw", "w", "nw", "n", "ne" };
        void PrintGraph(string str = "Graph")
        {
            str += ":\r\n";
            for (int i = 0; i < size.y; i++)
            {
                string l = "";
                for (int j = 0; j < size.x; j++)
                {
                    l += GetDir(graph[i, j].sur) + "\t";
                }
                str += l + "\r\n";
            }
            Debug.Log(str);
        }
        void PrintDepth(string str = "")
        {
            str += "Depth:\r\n";
            for (int i = 0; i < size.y; i++)
            {
                string l = (i + 1).ToString().PadRight(2) + " ";
                for (int j = 0; j < size.x; j++)
                {
                    //if (map[i, j] > 0)
                    //    l += (Symbol[(int)graph[i, j].value] + split).PadRightTo(padLen);
                    //else
                    //    l += (close[i, j].G.Keep(1).ToString() + split).PadRightTo(padLen);
                    if (map[i, j] > 0)
                        l += Symbol[(int)graph[i, j].value] + "\t";
                    else
                        l += close[i, j].G.Keep(fracCount).ToString() + "\t";
                }
                str += l + "\r\n";
            }
            Debug.Log(str);
        }
        void PrintSur()
        {
            string str = "Surround:\r\n";
            for (int i = 0; i < size.y; i++)
            {
                string l = "";
                for (int j = 0; j < size.x; j++)
                {
                    l += graph[i, j].sur;
                }
                str += l + "\r\n";
            }
            Debug.Log(str);
        }

        void PrintH()
        {
            string str = "H:\r\n";
            for (int i = 0; i < size.y; i++)
            {
                string l = "";
                for (int j = 0; j < size.x; j++)
                {
                    l += close[i, j].H;
                }
                str += l + "\r\n";
            }
            Debug.Log(str);
        }

        int PrintShortest()
        {
            int step = 0;
            var p = GetShortest();
            if (p == null) return 0;

            start = p;
            string str = "Path:\r\n";
            while (p.from != null)
            {
                graph[p.cur.pos.y, p.cur.pos.x].value = NodeType.Pass;
                str += p.cur.pos + "\r\n";
                p = p.from;
                step++;
            }
            graph[src.y, src.x].value = NodeType.Source;
            //Debug.Log(str); // 路径坐标列表
            return step;
        }
    }
}