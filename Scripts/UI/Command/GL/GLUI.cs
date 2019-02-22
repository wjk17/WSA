using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    /// <summary>
    /// 左下角坐标
    /// </summary>
    public static partial class GLUI
    {
        public static Material lineMaterial;
        public static Material texMaterial;
        internal static int commandOrder
        {
            set { _commandOrder = value; }
            get { var order = _commandOrder; if (!keepOrder) _commandOrder = 0; return order; }//每次使用后归0
                                                                                               //get {  return _commandOrder; }
        }

        internal static void LoadMatrix(Matrix4x4 matrix)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.LoadMatrix, matrix));
        }

        static int _commandOrder;
        public static float _secondOrder; // float, offen use z/depth
        public static int _insertOrder; // add cmd order
        static bool keepOrder = false;
        public static void BeginOrder(int order)
        {
            _commandOrder = order;
            _secondOrder = 0;
            //_insertOrder = 0;
            keepOrder = true;
        }
        public static void EndOrder()
        {
            _commandOrder = 0;
            keepOrder = false;
        }
        public static void SetLineMaterial()
        {
            if (!lineMaterial)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
            // Apply the line material
            lineMaterial.SetPass(0);
        }
        public static void SetTexMaterial(Texture texture)
        {
            if (!texMaterial) throw null;
            texMaterial.SetTexture("_MainTex", texture);
            texMaterial.SetPass(0);
        }
        public static void SetTexColor(Color color)
        {
            if (!texMaterial) throw null;
            texMaterial.SetColor("_Color", color);
            //texMaterial.SetPass(0);
        }
        public static void Test()
        {
            SetLineMaterial();
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(Vector2.zero);
            GL.Vertex(Vector2.one);
            GL.End();
        }
        public static GLCmd Cmd(int order, GLCmdType type, params object[] args)
        {
            var cmd = new GLCmd();
            cmd.order = order;
            cmd.type = type;
            cmd.args = args;
            return cmd;
        }
        public static void SetLineMat() // 正交变换
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.SetLineMat));
        }
        public static void PushMatrix() // 正交变换
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.PushMatrix));
        }
        public static void PopMatrix() // 正交变换
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.PopMatrix));
        }
        public static void LoadOrtho() // 正交变换
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.LoadOrtho));
        }
        public static void DrawTex(Texture2D texture, Vector2[] vs, Vector2[] uv)
        {
            DrawTex(texture, Color.white, vs, uv);
        }
        public static void DrawTex(Texture2D texture, Color color, Vector2[] vs, Vector2[] uv)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawTexOrtho, texture, color, vs, uv));
        }
        public static void DrawTex(Texture2D texture, params Vector2[] vs)
        {
            DrawTex(texture, (IList<Vector2>)vs);
        }
        public static void DrawTex(Texture2D texture, Color color, params Vector2[] v)
        {
            DrawTex(texture, color, (IList<Vector2>)v);
        }
        public static void DrawTex(Texture2D texture, IList<Vector2> v)
        {
            DrawTex(texture, Color.white, v);
        }
        public static void DrawTex(Texture2D texture, Color color, IList<Vector2> v)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawTexOrtho, texture, color, v[0], v[1], v[2], v[3]));
        }
        public static void DrawGrid(Vector3 gridSize, float smallStep, Color color)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawGrid, gridSize, smallStep, color));
        }
        /// <summary>
        /// Clockwise Reverse
        /// </summary>
        public static void _DrawTex(Texture2D texture, params Vector2[] v)
        {
            _DrawTex(texture, (IList<Vector2>)v);
        }
        public static void _DrawTex(Texture2D texture, Color color, params Vector2[] v)
        {
            _DrawTex(texture, color, (IList<Vector2>)v);
        }

        public static void _DrawGrid(Vector3 gridSize, float step, Color color)
        {
            SetLineMaterial();
            GL.Begin(GL.LINES);
            GL.Color(color);

            step = Mathf.Max(step, 0.01f);

            var lower = -gridSize * 0.5f;
            var upper = lower + gridSize;

            //X axis lines
            for (float z = lower.z; z <= upper.z; z += step)
            {
                GL.Vertex3(lower.x, 0, z);
                GL.Vertex3(upper.x, 0, z);
            }
            //Z axis lines
            for (float x = lower.x; x <= upper.x; x += step)
            {
                GL.Vertex3(x, 0, lower.z);
                GL.Vertex3(x, 0, upper.z);
            }
            GL.End();
        }
        public static void _DrawTex(Texture2D texture, IList<Vector2> v)
        {
            _DrawTex(texture, Color.white, v);
        }
        public static void _DrawTex(Texture2D texture, Color color, IList<Vector2> v)
        {
            SetTexMaterial(texture);
            SetTexColor(color);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            var uv = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)};
            for (int i = 0; i < 4; i++)
            {
                GL.TexCoord(new Vector3(uv[i].x, uv[i].y, texZ));
                GL.Vertex(v[i].ToNDC());
            }
            GL.End();
        }
        public static void _DrawTex(Texture2D texture, IList<Vector2> v, IList<Vector2> uv)
        {
            _DrawTex(texture, Color.white, v, uv);
        }
        public static float texZ;
        public static float uiZ;

        public static void _DrawTex(Texture2D texture, Color color, IList<Vector2> v, IList<Vector2> uv)
        {
            SetTexMaterial(texture);
            SetTexColor(color);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            for (int i = 0; i < 4; i++)
            {
                GL.TexCoord(new Vector3(uv[i].x, uv[i].y, texZ));
                GL.Vertex(v[i].ToNDC());
            }
            GL.End();
        }
        public static void DrawLineDirect(Vector3 p1, Vector3 p2, Color color)
        {
            DrawLineDirect(p1, p2, color, color);
        }
        public static void DrawLineDirect(Vector3 p1, Vector3 p2, Color color1, Color color2)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineDirect, p1, p2, color1, color2));
        }
        /// <summary>
        /// 左上角坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public static void DrawLine(Vector2 p1, Vector2 p2)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2));
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, Color color)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2, color));
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, Color color, bool clip)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2, color, clip));
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, float width)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2, width));
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, float width, Color color)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2, width, color));
        }
        public static void DrawLine(Vector2 p1, Vector2 p2, float width, Color color, bool clip)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawLineOrtho, p1, p2, width, color, clip));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="vs"></param>
        //public static void DrawQuad(Color color, IList<Vector3> vs)
        //{
        //    UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawQuadOrtho, color, ListTool.IListToArray(vs)));
        //}
        public static void DrawQuad(Color color, params Vector3[] vs)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawQuadOrtho, color, vs));
        }
        //v2
        public static void DrawQuad(IList<Vector2> vs, Color color)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawQuadOrtho, color, ListTool.IListToArray(vs)));
        }
        public static void DrawQuad(Color color, params Vector2[] vs)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawQuadOrtho, color, vs));
        }
        public static void DrawQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color color)
        {
            DrawQuad(color, p1, p2, p3, p4);
        }
        public static void DrawQuadDirect(Color color, params Vector3[] vs)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawQuadDirect, color, vs));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="width"></param>
        public static void DrawLineWidthIns(Vector2 p1, Vector2 p2, float width)
        {
            DrawLineWidthIns(p1, p2, width, Color.black);
        }
        public static void DrawLineWidthIns(Vector2 p1, Vector2 p2, float width, Color color)
        {
            GL.LoadOrtho();
            _DrawLineWidth(p1, p2, width, color, false);
        }
        public static void _DrawLineWidth(Vector2 p1, Vector2 p2, float width)
        {
            _DrawLineWidth(p1, p2, width, Color.black);
        }
        static void SortX(Vector2[] p)
        {
            SortX(ref p[0], ref p[1]);
        }
        static void SortY(Vector2[] p)
        {
            SortY(ref p[0], ref p[1]);
        }
        static void SortX(ref Vector2 p1, ref Vector2 p2)
        {
            if (p1.x > p2.x) Tool.swapPts(ref p1, ref p2);
        }
        static void SortY(ref Vector2 p1, ref Vector2 p2)
        {
            if (p1.y > p2.y) Tool.swapPts(ref p1, ref p2);
        }
        // 控制粗细的线条实际是画四边形，不一定与坐标轴垂直。
        public static void _DrawLineWidth(Vector2 p1, Vector2 p2, float width, Color color, bool clip = true)
        {
            SetLineMaterial();
            //clip
            if (clip)
            {
                var RT = UI.owner as RectTransform;
                if (RT != null)
                {
                    var rect = RT.Rect();
                    if (LineClip.ClipCohSuth(rect[0], rect[1], ref p1, ref p2) == LineClip.Result.discard) return;
                }
            }
            var v = p2 - p1;
            var v2 = p1 - p2;
            width *= 0.5f;
            var p1a = p1 + new Vector2(-v.y, v.x).normalized * width;
            var p1b = p1 + new Vector2(v.y, -v.x).normalized * width;
            var p2a = p2 + new Vector2(-v2.y, v2.x).normalized * width;
            var p2b = p2 + new Vector2(v2.y, -v2.x).normalized * width;

            //四边形可能切成更多边形，暂时没做画多边形功能因此暂不裁剪宽度，只裁剪长度
            _DrawQuad(color, p1a, p1b, p2a, p2b);
        }
        // 左下角原点（0,0），右上角（1,1）
        public static void _DrawLineDirect(Vector2 p1, Vector2 p2)
        {
            _DrawLineDirect(p1, p2, Color.black);
        }
        public static void _DrawLineDirect(Vector3 p1, Vector3 p2, Color color)
        {
            _DrawLineDirect(p1, p2, color, color);
        }
        public static void _DrawLineDirect(Vector3 p1, Vector3 p2, Color color1, Color color2)
        {
            SetLineMaterial();
            GL.Begin(GL.LINES);
            GL.Color(color1);
            GL.Vertex(p1);
            GL.Color(color2);
            GL.Vertex(p2);
            GL.End();
        }
        /// <summary>
        /// Ref Pos
        /// </summary>
        public static void _DrawLineOrtho(Vector2 p1, Vector2 p2)
        {
            _DrawLineOrtho(p1, p2, Color.black);
        }
        public static void _DrawLineOrtho(Vector2 p1, Vector2 p2, Color color, bool clip = true)
        {
            _DrawLineOrtho(p1, p2, color, color, clip);
        }
        public static void _DrawLineOrtho(Vector2 p1, Vector2 p2, Color color1, Color color2, bool clip = true)
        {
            SetLineMaterial();
            //clip
            if (clip)
            {
                var RT = UI.owner as RectTransform;
                if (RT != null)
                {
                    var rect = RT.Rect();
                    if (LineClip.ClipCohSuth(rect[0], rect[1], ref p1, ref p2) == LineClip.Result.discard) return;
                }
            }
            //normalize & flip y
            p1 = p1.ToNDC();
            p2 = p2.ToNDC();

            GL.Begin(GL.LINES);
            GL.Color(color1);
            GL.Vertex(p1);
            GL.Color(color2);
            GL.Vertex(p2);
            GL.End();
        }
        public static void DrawSquareSoild()
        {
        }
        // 垂直于坐标轴的正方形
        internal static void DrawSquare(Vector2 p, float wh)
        {
            DrawSquare(p, wh, Color.black);
        }
        internal static void DrawSquare(Vector2 p, float wh, Color color)
        {
            SetLineMaterial();
            wh *= 0.5f;
            var p1 = p - Vector2.one * wh; // lt
            var p2 = new Vector2(p.x + wh, p.y - wh); // rt
            var p3 = p + Vector2.one * wh; // rb
            var p4 = new Vector2(p.x - wh, p.y + wh); // lb
            DrawLine(p1, p2, color);
            DrawLine(p2, p3, color);
            DrawLine(p3, p4, color);
            DrawLine(p4, p1, color);
        }
        internal static void DrawSquare(Vector2 p, float wh, float lineWidth)
        {
            DrawSquare(p, wh, lineWidth, Color.black);
        }
        internal static void DrawSquare(Vector2 p, float wh, float lineWidth, Color color)
        {
            SetLineMaterial();
            wh *= 0.5f;
            var p12a = p - Vector2.one * wh;
            var p12b = new Vector2(p.x + wh, p.y - wh);
            var p23a = new Vector2(p.x + wh, p.y - wh);
            var p23b = p + Vector2.one * wh;
            var p34a = p + Vector2.one * wh;
            var p34b = new Vector2(p.x - wh, p.y + wh);
            var p41a = new Vector2(p.x - wh, p.y + wh);
            var p41b = p - Vector2.one * wh;
            lineWidth *= 0.5f;
            var width = lineWidth * 0.5f;
            p12a.x -= width;
            p12b.x += width;
            p34a.x += width;
            p34b.x -= width;
            DrawLine(p12a, p12b, lineWidth, color);
            DrawLine(p23a, p23b, lineWidth, color);
            DrawLine(p34a, p34b, lineWidth, color);
            DrawLine(p41a, p41b, lineWidth, color);
        }
        public static void _DrawQuad(Color color, params Vector2[] vs)
        {
            SetLineMaterial();
            GL.Begin(GL.QUADS);
            GL.Color(color);
            foreach (var v in vs)
            {
                GL.Vertex(v.ToNDC().SetZ(uiZ));
            }
            GL.End();
        }
        public static void _DrawQuad(Color color, params Vector3[] vs)
        {
            SetLineMaterial();
            GL.Begin(GL.QUADS);
            GL.Color(color);
            foreach (var v in vs)
            {
                GL.Vertex(v.XYToNDC());
            }
            GL.End();
        }
        public static void _DrawQuadDirect(Color color, params Vector3[] vs)
        {
            SetLineMaterial();
            GL.Begin(GL.QUADS);
            GL.Color(color);
            foreach (var v in vs)
            {
                GL.Vertex(v);
            }
            GL.End();
        }
        public static void _DrawQuadDirect(Color[] color, Vector3[] vs)
        {
            SetLineMaterial();
            GL.Begin(GL.QUADS);
            for (int i = 0; i < vs.Length; i++)
            {
                GL.Color(color[i]);
                GL.Vertex(vs[i]);
            }
            GL.End();
        }
        public static void _DrawQuadDirect_CusMat(Color[] color, Vector3[] vs)
        {
            GL.Begin(GL.QUADS);
            for (int i = 0; i < vs.Length; i++)
            {
                GL.Color(color[i]);
                GL.Vertex(vs[i]);
            }
            GL.End();
        }
        public static void DrawCircle(Vector3 pos, float radius, Color color, float accurracy = 0.01f)
        {
            DrawCircle(pos.x, pos.y, pos.z, radius, color, accurracy);
        }
        static void DrawCircle(float x, float y, float z, float r, Color color, float accuracy)
        {
            SetLineMaterial();
            float stride = r * accuracy;
            float size = 1 / accuracy;
            float x1 = x, x2 = x, y1 = 0, y2 = 0;
            float x3 = x, x4 = x, y3 = 0, y4 = 0;

            double squareDe;
            squareDe = r * r - Math.Pow(x - x1, 2);
            squareDe = squareDe > 0 ? squareDe : 0;
            y1 = (float)(y + Math.Sqrt(squareDe));
            squareDe = r * r - Math.Pow(x - x1, 2);
            squareDe = squareDe > 0 ? squareDe : 0;
            y2 = (float)(y - Math.Sqrt(squareDe));
            for (int i = 0; i < size; i++)
            {
                x3 = x1 + stride;
                x4 = x2 - stride;
                squareDe = r * r - Math.Pow(x - x3, 2);
                squareDe = squareDe > 0 ? squareDe : 0;
                y3 = (float)(y + Math.Sqrt(squareDe));
                squareDe = r * r - Math.Pow(x - x4, 2);
                squareDe = squareDe > 0 ? squareDe : 0;
                y4 = (float)(y - Math.Sqrt(squareDe));

                //绘制线段
                GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex(new Vector3(x1 / Screen.width, y1 / Screen.height, z));
                GL.Vertex(new Vector3(x3 / Screen.width, y3 / Screen.height, z));
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex(new Vector3(x2 / Screen.width, y1 / Screen.height, z));
                GL.Vertex(new Vector3(x4 / Screen.width, y3 / Screen.height, z));
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex(new Vector3(x1 / Screen.width, y2 / Screen.height, z));
                GL.Vertex(new Vector3(x3 / Screen.width, y4 / Screen.height, z));
                GL.End();
                GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height, z));
                GL.Vertex(new Vector3(x4 / Screen.width, y4 / Screen.height, z));
                GL.End();

                x1 = x3;
                x2 = x4;
                y1 = y3;
                y2 = y4;
            }
        }
    }
}