using System.Collections.Generic;
using UnityEngine;

namespace Esa.UI
{
    // UI控件
    public partial class UITimeLine : Singleton<UITimeLine>
    {
        public float fps = 60;
        public Color clrTimeLine = Color.green;
        public Color clrAxis = Color.black;
        public Color clrGrid = Palette.L1;

        public int xSpaceTextInRuler; // 标尺每隔多少帧有一个帧数数字
        public int xSpaceLineInRuler; // 多少帧画一条线
        public float rulerScalerSensitivity = 20;

        public int indexNAccuracy = 3;

        public Vector2 START = Vector2.zero;
        public Vector2 SIZE = new Vector2(20, 1);
        public float gridFactorText;
        public float gridFactorLine;
        public int maxError = 3;
        Vector2Int SizeInt { get { return SIZE.ToInt(); } }

        public float width = 3f;
        public bool clip = false;
        private void Update()
        {
            this.FrameStart();
            this.Draw(Color.grey);
            var pos = UI.AbsRefPos(rtArea) - rtArea.rect.size * 0.5f;
            var scl = rtAreaSize / SizeInt;

            var os = Matrix4x4.Translate(START);
            var m_CurveToRef = Matrix4x4.TRS(pos, Quaternion.identity, scl) * os;
            var m_RulerToRef = Matrix4x4.TRS(UI.AbsRefPos(ruler) - rulerSize.X() * 0.5f
                , Quaternion.identity, rtSize / SizeInt) * os;

            var a = Vector2.zero;
            var b = Vector2.up * SIZE.y;
            var f = 1f / SIZE.x;

            xSpaceTextInRuler = Mathf.RoundToInt(SIZE.x * gridFactorText);
            var fitted = (xSpaceTextInRuler / 5) * 5;
            if (Mathf.Abs(xSpaceTextInRuler - fitted) > maxError)
                fitted = (xSpaceTextInRuler / 2) * 2;

            xSpaceTextInRuler = Mathf.Max(2, fitted);


            xSpaceLineInRuler = Mathf.RoundToInt(SIZE.x * gridFactorLine);
            fitted = (xSpaceLineInRuler / 5) * 5;
            if (Mathf.Abs(xSpaceLineInRuler - fitted) > maxError)
                fitted = (xSpaceLineInRuler / 2) * 2;

            xSpaceLineInRuler = Mathf.Max(2, xSpaceLineInRuler);

            // grid
            for (int i = Mathf.RoundToInt(-START.x); i < -START.x + SIZE.x; i++)
            {
                if ((i % xSpaceLineInRuler) == 0)
                {
                    a.x = b.x = i; // 曲线空间坐标
                    DrawLine(a, b, I.clrGrid, m_CurveToRef);
                }
                if ((i % xSpaceTextInRuler) == 0)
                {
                    a.x = i + f * 0.5f;
                    Vector2 c = m_RulerToRef.MultiplyPoint(a);
                    c = rulerPos + c.ToLT();
                    IMUI.DrawText(i.ToString(), c, Vectors.half2d);// 画字 帧号标签
                }
            }
            // timeline
            GLUI.BeginOrder(5);
            b.x = a.x = frameIdx;
            DrawLine(a, b, I.clrTimeLine, m_CurveToRef, width);
            // axis
            GLUI.BeginOrder(2);
            b.x = a.x = 0;
            DrawLine(a, b, I.clrAxis, m_CurveToRef, width);
        }
        void DrawLine(Vector2 a, Vector2 b, Color color, Matrix4x4 m, float width = 1f)
        {
            a = m.MultiplyPoint(a);
            b = m.MultiplyPoint(b);
            if (width == 1) GLUI.DrawLine(a, b, color, clip);
            else GLUI.DrawLine(a, b, width, color, clip);
        }
        void Start()
        {
            this.AddInput(GetInput, -5, false);
            frameIdx_KeyHandler = new FrameIdx_Key(UICurve.I.curveSel);
            frameIdx = 0;
        }
        void GetInput()
        {
            frameIdx_KeyHandler.curve = UICurve.I.curveSel;
            var use = false;
            float delta = Events.AxisMouseWheel;
            if (delta != 0 && UI.MouseOver(rt, ruler, UICurve.I.rt))
            {
                use = true;
                SIZE.x -= delta * rulerScalerSensitivity;
                SIZE.x = Mathf.Clamp(SIZE.x, 10, Mathf.Infinity);
            }
            frameIdx = frameIdx_KeyHandler.GetInput(frameIdx);
            if (Events.KeyDown(KeyCode.I))
            {
                if (Events.Alt)
                    RemoveKey();
                else InsertKey();
            }
            if (use) Events.Use();
        }
        public void RemoveKey()
        {
            RemoveKeyAt(frameIdx);
        }
        public void RemoveKeyAt(float time)
        {
            switch (insertType)
            {
                case InsertKeyType.EulPos: UIClip.I.clip.RemoveEulerPosAllCurve(time); break;
                case InsertKeyType.Eul: break;
                case InsertKeyType.Pos: break;
                default: throw null;
            }
            ClipTool.GetFrameRange(UIClip.I.clip);
        }
        public void InsertKey()
        {
            switch (insertType)
            {
                case InsertKeyType.EulPos:
                    UIClip.I.clip.AddEulerPosAllCurve(frameIdx);
                    break;
                case InsertKeyType.Eul:
                    UIClip.I.clip.AddEulerAllCurve(frameIdx);
                    UIClip.I.clip.AddPosCurve(frameIdx, Bone.hips, Bone.root);
                    break;
                case InsertKeyType.Pos:
                    UIClip.I.clip.AddPosAllCurve(frameIdx);
                    break;
                default: throw null;
            }
            ClipTool.GetFrameRange(UIClip.I.clip);
        }
    }
}