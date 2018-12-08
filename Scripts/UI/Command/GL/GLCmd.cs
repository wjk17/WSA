using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI
{
    public enum GLCmdType
    {
        LoadOrtho,
        DrawLineOrtho,
        DrawQuadOrtho,
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