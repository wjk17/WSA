using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.Pathing
{
    public partial class AStar_0
    {
        public int MaxLength = 100;    //用于优先队列（Open表）的数组
        public Vector2Int size = new Vector2Int(15, 15); // 地图尺寸

        enum Result
        {
            Sequential = 0,  //顺序遍历
            NoSolution = 2,  //无解决方案
        }
        const int Infinity = 0xfffffff;

        const int East = (1 << 0);
        const int South_East = (1 << 1);
        const int South = (1 << 2);
        const int South_West = (1 << 3);
        const int West = (1 << 4);
        const int North_West = (1 << 5);
        const int North = (1 << 6);
        const int North_East = (1 << 7);

        Vector2Int[] dir;

        public Vector2Int src, dst;

        MapNode[,] graph;
        Close[,] close;
        public Open open;

        Close start;
        public int shortestep;

        public int[,] map;
        [Serializable]
        public class MapNode
        {
            public Vector2Int pos;
            public bool reachable;
            public int sur;
            public NodeType value;
        }
        [Serializable]
        public class Close
        {
            public MapNode cur;
            public bool vis; // 是否被访问
            public Close from; // 所来节点
            public float F, G;
            /// <summary>
            /// 曼哈顿距离
            /// </summary>
            public int H; // 评价函数值
        }
        [Serializable]
        public class Open //优先队列（Open表）
        {
            public int length;        //当前队列的长度
            public List<Close> list;    //评价结点的指针
            public Open(int MaxLength) //优先队列初始化
            {
                list = new List<Close>();
                for (int i = 0; i < MaxLength; i++)
                {
                    list.Add(null);
                }
                length = 0; // 队内元素数初始为0
            }
        }
        // 地图Close表初始化配置
        void InitClose(Vector2Int src, Vector2Int dst)
        {
            for (int i = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    close[i, j] = new Close();
                    close[i, j].cur = graph[i, j];          // Close表所指节点
                    close[i, j].vis = !graph[i, j].reachable;        // 是否被访问
                    close[i, j].from = null;                // 所来节点
                    close[i, j].G = close[i, j].F = 0;
                    // 评价函数值（与目标点的曼哈顿距离）
                    close[i, j].H = dst.Manhattan(new Vector2Int(j, i));
                }
            }
            close[src.y, src.x].G = 0;                       //移步花费代价值
            close[src.y, src.x].F = 0 + close[src.y, src.x].H;  //起始点评价初始值
            close[src.y, src.x].vis = true; // 起始点会被第一个访问

            close[dst.y, dst.x].G = Infinity;
        }
        void InitGraph()
        {
            graph = new MapNode[size.y, size.x];
            close = new Close[size.y, size.x];
            //地图发生变化时重新构造地
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    graph[y, x] = new MapNode();
                    graph[y, x].pos.y = y; //地图坐标X
                    graph[y, x].pos.x = x; //地图坐标Y
                    graph[y, x].value = (NodeType)map[y, x];
                    graph[y, x].reachable = (graph[y, x].value == NodeType.Reachable); // 节点可到达性
                    graph[y, x].sur = 0; //邻接节点个数
                    if (!graph[y, x].reachable)
                    {
                        continue;
                    }
                    if (x > 0)
                    {
                        if (graph[y, x - 1].reachable)    // left节点可以到达
                        {
                            graph[y, x].sur |= West;
                            graph[y, x - 1].sur |= East;
                        }
                        if (y > 0)
                        {
                            if (graph[y - 1, x - 1].reachable
                                && graph[y - 1, x].reachable    // 左，上，左上都可到达时
                                && graph[y, x - 1].reachable)    // up-left节点可以到达
                            {
                                graph[y, x].sur |= North_West;
                                graph[y - 1, x - 1].sur |= South_East;
                            }
                        }
                    }
                    if (y > 0)
                    {
                        if (graph[y - 1, x].reachable)    // up节点可以到达
                        {
                            graph[y, x].sur |= North;
                            graph[y - 1, x].sur |= South;
                        }
                        if (x < size.x - 1)
                        {
                            //print(y - 1);
                            //print(x + 1);
                            //print("0:" + graph.GetUpperBound(0));
                            //print("1:" + graph.GetUpperBound(1));
                            if (graph[y - 1, x + 1].reachable
                                && graph[y - 1, x].reachable  // 上，右，上右都可到达时
                                && (NodeType)map[y, x + 1] == NodeType.Reachable) // up-right节点可以到达
                            {
                                graph[y, x].sur |= North_East;
                                graph[y - 1, x + 1].sur |= South_West;
                            }
                        }
                    }
                }
            }
            graph[src.y, src.x].value = NodeType.Source;
            graph[dst.y, dst.x].value = NodeType.Destination;
        }
        // Clear Map Marks of Steps
        void ClearMap()
        {
            Close p = start;
            while (p != null)
            {
                graph[p.cur.pos.y, p.cur.pos.x].value = NodeType.Reachable;
                p = p.from;
            }
            graph[src.y, src.x].value = (NodeType)map[src.y, src.x];
            graph[dst.y, dst.x].value = (NodeType)map[dst.y, dst.x];
        }
        void InitMap()
        {
            dir = new Vector2Int[]
            {
                new Vector2Int(0, 1),   // East
                new Vector2Int(1, 1),   // South_East
                new Vector2Int(1, 0),   // South
                new Vector2Int(1, -1),  // South_West
                new Vector2Int(0, -1),  // West
                new Vector2Int(-1, -1), // North_West
                new Vector2Int(-1, 0),  // North
                new Vector2Int(-1, 1)   // North_East
            };
            for (int i = 0; i < dir.Length; i++)
            {
                var t = dir[i].x;
                dir[i].x = dir[i].y;
                dir[i].y = t;
            }

            map = new int[,]{
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
            {0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,1,1},
            {0,0,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1},
            {0,0,0,0,0,0,1,0,0,0,0,0,0,1,1,0,0,0,0,1},
            {0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,1},
            {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,1,0,0,0,0,1,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0},
            {0,1,0,0,0,0,1,0,0,0,0,0,0,1,0,1,0,0,0,1},
            {0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0}
            };
        }
    }
}