using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public enum GLCmdType
    {
        SetLineMat,
        LoadOrtho,
        LoadMatrix,
        DrawGrid,
        DrawLineOrtho,
        DrawQuadOrtho,
        DrawTexOrtho,
    }
    [Serializable]
    public class GLCmd : Cmd
    {
        public GLCmdType type;
        public override string ToString()
        {
            var typeName = Enum.GetName(typeof(GLCmdType), type);
            return typeName + " " + base.ToString();
        }
    }    
}