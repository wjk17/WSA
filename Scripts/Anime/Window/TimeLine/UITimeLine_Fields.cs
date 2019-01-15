using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Esa._UI
{
    public enum InsertKeyType
    {
        EulPos,
        Eul,
        Pos,
    }
    public partial class UITimeLine//_Fields
    {
        public InsertKeyType insertType;
        FrameIdx_Key frameIdx_KeyHandler;
        public Text txtFrameIdx;
        public Text txtFrameIdxN;
        /// <summary>
        /// range start end
        /// </summary>        
        public Vector2Int startPosInt
        {
            get { return startPos.RoundToInt(); }
        }
        Vector2 startPos;
        public Vector2Int endPosInt
        {
            get { return endPos.RoundToInt(); }
        }
        Vector2 endPos;
        public Vector2 range
        {
            get { return endPos - startPos; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int frameIdx
        {
            get { return Mathf.RoundToInt(frameIdx_F); }
            set
            {
                if (frameIdx != value)
                {
                    frameIdx_F = value;
                }
            }
        }
        float _frameIdx_F;
        public Action<int> onFrameIdxChanged;
        public float frameIdx_F
        {
            get { return _frameIdx_F; }
            set
            {
                bool changed = !_frameIdx_F.Approx(value, 3);
                _frameIdx_F = value;
                txtFrameIdx.text = "帧：" + frameIdx.ToString();
                txtFrameIdxN.text = "n：" + frameIdxN.ToString("0.00");
                if (changed && onFrameIdxChanged != null) onFrameIdxChanged(frameIdx);                
            }
        }
        public float frameIdxN
        {
            get
            {
                return 0;
                //var end = UIClip.I.clip.frameRange.y;
                //return end == 0 ? 0 : MathTool.Round(frameIdx_F / end, indexNAccuracy);
            }
        }
    }
}