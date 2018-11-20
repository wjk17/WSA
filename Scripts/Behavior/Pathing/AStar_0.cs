using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa.Pathing
{
    public partial class AStar_0 : MonoBehaviour
    {
        public Close[] bfs_Open;
        public bool useH;
        public bool printOpen;

        // 宽度优先搜索（Breadth First Search）
        int BFSearch()
        {
            int times = 0;
            int f = 0, r = 1;
            bfs_Open = new Close[MaxLength];
            bfs_Open[0] = close[src.y, src.x];

            InitClose(src, dst);

            while (r != f)
            {
                var p = bfs_Open[f];
                f = (f + 1) % MaxLength;
                var cur = p.cur.pos;
                for (int i = 0; i < 8; i++)
                {
                    if ((p.cur.sur & (1 << i)) == 0)
                    {
                        continue; // 这个方向没有邻近点
                    }
                    var sur = cur + dir[i];
                    var node = close[sur.y, sur.x];
                    if (!node.vis) // 如果这个邻近点未访问
                    {
                        node.from = p;
                        node.vis = true;
                        node.G = p.G + 1;  // 评价+1
                        bfs_Open[r] = node;   // 则Push到待访问列表
                        r = (r + 1) % MaxLength;
                    }
                }
                times++;
            }
            return times;
        }
        //向优先队列（Open表）中添加元素
        void Push(Open q, Vector2Int pos, float g)
        {
            var node = close[pos.y, pos.x];
            node.G = g;    //所添加节点的坐标            
            node.F = g + node.H;
            q.list[q.length++] = node;
            var mintag = q.length - 1;
            for (int i = 0; i < q.length - 1; i++)
            {
                if ((!useH && q.list[i].G < q.list[mintag].G) ||
                    (useH && q.list[i].F < q.list[mintag].F))
                {
                    mintag = i;
                }
            }
            q.list.Swap(q.length - 1, mintag); // 将评价函数值最小节点置于队头
        }
        Close Shift(Open q)
        {
            return q.list[--q.length];
        }
        void PrintOpen()
        {
            var str = "";
            for (int i = 0; i < open.length; i++)
            {
                str += open.list[i].G + ", ";
            }
            print(str);
        }
        // A* 算法遍历
        Result AStar()
        {
            open = new Open(MaxLength); //Open表

            InitClose(src, dst); // 初始化
            Push(open, src, 0); // 从起点开始
            var tick = 0;
            while (open.length > 0)
            {
                var p = Shift(open); // 从Open表末尾拿出一个节点
                var cur = p.cur.pos;
                if (p.H == 0) // 完成
                {
                    return Result.Sequential;
                }
                for (int i = 0; i < 8; i++)
                {
                    if ((p.cur.sur & (1 << i)) == 0)
                    {
                        continue;
                    }
                    var sur = cur + dir[i];
                    if (!close[sur.y, sur.x].vis)
                    {
                        close[sur.y, sur.x].vis = true;
                        close[sur.y, sur.x].from = p;
                        var surG = p.G + 1;// Vector2.Distance(sur, cur);
                        Push(open, sur, surG);
                    }
                }
                if (printOpen) PrintOpen();
                //PrintMap(tick++.ToString());
                //PrintDepth(tick++.ToString() + " ");
            }
            return Result.NoSolution; //无结果
        }
        // 获取最短路径
        Close GetShortest()
        {
            Close p, t, q = null;
            switch (AStar())
            {
                case Result.Sequential:    //顺序最近
                    p = close[dst.y, dst.x];
                    while (p != null)    //转置路径（逆转方向）
                    {
                        t = p.from;
                        p.from = q;
                        q = p;
                        p = t;
                    }
                    close[src.y, src.x].from = q.from;
                    return close[src.y, src.x];
                case Result.NoSolution:
                    return null;
            }
            return null;
        }
        [Button]
        void Start()
        {
            InitMap();
            InitGraph();
            PrintMap("Map");
            PrintGraph();

            var min = Vector2Int.zero;
            var max = size - Vector2Int.one;
            src = VectorTool.Clamp(src, min, max);
            dst = VectorTool.Clamp(dst, min, max);

            shortestep = PrintShortest();
            if (shortestep > 0)
            {
                PrintMap("Path（Steps " + shortestep + "）");
                PrintDepth();
                ClearMap();

                // 用宽度优先搜索验证
                //print("BFSearch: " + BFSearch());
                BFSearch();
                PrintDepth();
                Debug.Log((shortestep == close[dst.y, dst.x].G) ? "正确" : "错误");
            }
            else Debug.Log("不可到达");
        }
    }
}
