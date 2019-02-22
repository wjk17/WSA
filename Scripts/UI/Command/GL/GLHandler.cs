using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI_
{
    [Serializable]
    public class GLHandler : CmdHandler
    {
        public int order;
        public string name;
        public GLHandler() : base() { }
        public GLHandler(object owner) : base(owner) { }
        public override void ExecuteCommand(Cmd command)
        {
            var cmd = command as GLCmd;
            switch (cmd.type)
            {
                case GLCmdType.LoadOrtho: GL.LoadOrtho(); break;
                case GLCmdType.LoadMatrix: GL.LoadProjectionMatrix((Matrix4x4)cmd.args[0]); break;
                case GLCmdType.PushMatrix: GL.PushMatrix(); break;
                case GLCmdType.PopMatrix: GL.PopMatrix(); break;
                case GLCmdType.SetLineMat: GLUI.SetLineMaterial(); break;
                case GLCmdType.SetFontColor:
                    if (ArgType<Color>(cmd))
                    {
                        GLUI._SetFontColor((Color)cmd.args[0]);
                    }
                    break;
                case GLCmdType.DrawString:
                    if (ArgType<Vector2, string, int, Vector2>(cmd))
                    {
                        GLUI._DrawString((Vector2)cmd.args[0], (string)cmd.args[1], (int)cmd.args[2], (Vector2)cmd.args[3]);
                    }
                    else Debug.Log("DrawString Error");
                    break;
                case GLCmdType.DrawGrid:
                    if (ArgType<Vector3, float, Color>(cmd))
                    {
                        GLUI._DrawGrid((Vector3)cmd.args[0], (float)cmd.args[1], (Color)cmd.args[2]);
                    }
                    else Debug.Log("DrawGrid Error");
                    break;
                case GLCmdType.DrawLineDirect:
                    if (ArgType<Vector3, Vector3, Color, Color>(cmd))
                    {
                        GLUI._DrawLineDirect((Vector3)cmd.args[0], (Vector3)cmd.args[1], (Color)cmd.args[2], (Color)cmd.args[3]);
                    }
                    else if (ArgType<Vector3, Vector3, Color>(cmd))
                    {
                        GLUI._DrawLineDirect((Vector3)cmd.args[0], (Vector3)cmd.args[1], (Color)cmd.args[2]);
                    }
                    else Debug.Log("Error");
                    break;
                case GLCmdType.DrawLineOrtho:
                    if (ArgType<Vector2, Vector2>(cmd))
                    {
                        GLUI._DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1]);
                    }
                    else if (ArgType<Vector2, Vector2, Color>(cmd))
                    {
                        GLUI._DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1], (Color)cmd.args[2]);
                    }
                    else if (ArgType<Vector2, Vector2, Color, bool>(cmd))
                    {
                        GLUI._DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1], (Color)cmd.args[2], (bool)cmd.args[3]);
                    }
                    else if (ArgType<Vector2, Vector2, float>(cmd))
                    {
                        GLUI._DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2]);
                    }
                    else if (ArgType<Vector2, Vector2, float, Color>(cmd))
                    {
                        GLUI._DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2], (Color)cmd.args[3]);
                    }
                    else if (ArgType<Vector2, Vector2, float, Color, bool>(cmd))
                    {
                        GLUI._DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2], (Color)cmd.args[3], (bool)cmd.args[4]);
                    }
                    else { throw new Exception("未定义 参数"); }
                    break;
                case GLCmdType.DrawQuadDirect:
                    if (ArgType<Color, Vector3[]>(cmd))
                    {
                        GLUI._DrawQuadDirect((Color)cmd.args[0], (Vector3[])cmd.args[1]);
                    }
                    else { throw new Exception("未定义 参数"); }
                    break;
                case GLCmdType.DrawQuadOrtho:
                    if (ArgType<Color, Vector3[]>(cmd))
                    {
                        GLUI._DrawQuad((Color)cmd.args[0], (Vector3[])cmd.args[1]);
                    }
                    else if (ArgType<Color, Vector2[]>(cmd))
                    {
                        GLUI._DrawQuad((Color)cmd.args[0], (Vector2[])cmd.args[1]);
                    }
                    else { throw new Exception("未定义 参数"); }
                    break;
                case GLCmdType.DrawTexOrtho:
                    if (ArgType<Texture2D, Color, Vector2[], Vector2[]>(cmd))
                    {
                        GLUI._DrawTex((Texture2D)cmd.args[0], (Color)cmd.args[1],
                            (Vector2[])cmd.args[2],
                            (Vector2[])cmd.args[3]);
                    }
                    else if (ArgType<Texture2D, Color, Vector2, Vector2, Vector2, Vector2>(cmd))
                    {
                        GLUI._DrawTex((Texture2D)cmd.args[0], (Color)cmd.args[1],
                            (Vector2)cmd.args[2],
                            (Vector2)cmd.args[3],
                            (Vector2)cmd.args[4],
                            (Vector2)cmd.args[5]);
                    }
                    else
                    {
                        string str = "";
                        foreach (var arg in cmd.args)
                        {
                            str += (str.Length > 0 ? ", " : "") + arg.GetType().Name;
                        }
                        throw new Exception("undef(" + cmd.args.Length + "): " + str);
                    }
                    break;
                default: throw new Exception("未定义 命令");
            }
        }
    }
}