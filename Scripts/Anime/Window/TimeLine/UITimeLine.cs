using System.Collections.Generic;
using UnityEngine;

namespace Esa.UI
{
    // UI控件
    public partial class UITimeLine : Singleton<UITimeLine>
    {
        public float fps = 60;
        public Color clrTimeLine = Color.green;
        public Color clrGrid = Palette.L1;

        public int xSpaceTextInRuler; // 标尺每隔多少帧有一个帧数数字
        public int xSpaceLineInRuler; // 多少帧画一条线
        public float rulerScalerSensitivity = 20;

        public int indexNAccuracy = 3;

        public Vector2 c;
        public Vector2 SIZE = new Vector2(20, 15);
        Vector2Int SizeInt { get { return SIZE.ToInt(); } }

        public float width = 3f;
        public bool clip = false;
        private void Update()
        {
            this.FrameStart();
            this.Draw(Color.grey);
            var pos = UI.AbsRefPos(rtArea) - rtArea.rect.size * Vectors.half2d;
            var scl = rtAreaSize / SizeInt;

            var m_AreaToRect = Matrix4x4.TRS(pos, Quaternion.identity, scl);

            var m_RectToRef = Matrix4x4.TRS(rtPos, Quaternion.identity, rtSize / SizeInt);
            var m_RulerToRef = Matrix4x4.TRS(rtPos, Quaternion.identity, rtSize / SizeInt);

            var a = Vector2.zero;
            var b = Vector2.up * SIZE.y;
            var f = 1f / SIZE.x;

            // grid
            for (int i = 0; i < SIZE.x; i++)
            {
                if ((i % xSpaceLineInRuler) == 0)
                {
                    a.x = b.x = i; // 曲线空间坐标
                    DrawLine(a, b, I.clrGrid, m_AreaToRect);
                }
                if ((i % xSpaceTextInRuler) == 0)
                {
                    a.x = i + f * 0.5f;
                    c = m_RectToRef.MultiplyPoint(a);
                    c = rulerPos + c.ToLT();
                    IMUI.DrawText(i.ToString(), c, Vectors.half2d);// 画字 帧号标签
                }
            }
            // timeline
            GLUI.BeginOrder(1);
            if (frameIdx.Between(SizeInt.x))
            {
                b.x = a.x = frameIdx;
                DrawLine(a, b, I.clrTimeLine, m_AreaToRect, width);
            }
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
            frameIdx_KeyHandler = new FrameIdx_Key(UICurve.Curve);
            frameIdx = 0;
        }
        void GetInput()
        {
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
                case InsertKeyType.EulPos: UIClip.I.clip.AddEulerPosAllCurve(frameIdx); break;
                case InsertKeyType.Eul: break;
                case InsertKeyType.Pos: break;
                default: throw null;
            }
            ClipTool.GetFrameRange(UIClip.I.clip);
        }
    }
}