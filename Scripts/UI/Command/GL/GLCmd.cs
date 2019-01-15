using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa._UI
{
    public enum GLCmdType
    {
        SetLineMat,
        PushMatrix,
        PopMatrix,
        LoadOrtho,
        LoadMatrix,
        DrawGrid,
        DrawLineWorld,
        DrawLineOrtho,
        DrawQuadOrtho,
        DrawQuadDirect,
        DrawTexOrtho,
    }
    [Serializable]
    public class GLCmd : Cmd
    {
        public GLCmdType type;
        public override string ToString()
        {
            var typeName = Enum.GetName(typeof(GLCmdType), type);
            return "[" + order + "]" + typeName + " " + base.ToString();
        }
    }
}