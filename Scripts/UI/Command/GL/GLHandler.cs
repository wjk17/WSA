using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI
{
    [Serializable]
    public class GLHandler : CmdHandler
    {
        public GLHandler() : base() { }
        public GLHandler(RectTransform owner) : base(owner) { }
        public override void ExecuteCommand(Cmd command)
        {
            var cmd = command as GLCmd;
            switch (cmd.type)
            {
                case GLCmdType.LoadOrtho: GL.LoadOrtho(); break;
                case GLCmdType.SetLineMat: GLUI.SetLineMaterial(); break;
                case GLCmdType.DrawLineOrtho:
                    if (ArgType<Vector2, Vector2>(cmd))
                    {
                        GLUI.DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1]);
                    }
                    else if (ArgType<Vector2, Vector2, Color>(cmd))
                    {
                        GLUI.DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1], (Color)cmd.args[2]);
                    }
                    else if (ArgType<Vector2, Vector2, Color, bool>(cmd))
                    {
                        GLUI.DrawLineOrtho((Vector2)cmd.args[0], (Vector2)cmd.args[1], (Color)cmd.args[2], (bool)cmd.args[3]);
                    }
                    else if (ArgType<Vector2, Vector2, float>(cmd))
                    {
                        GLUI.DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2]);
                    }
                    else if (ArgType<Vector2, Vector2, float, Color>(cmd))
                    {
                        GLUI.DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2], (Color)cmd.args[3]);
                    }
                    else if (ArgType<Vector2, Vector2, float, Color, bool>(cmd))
                    {
                        GLUI.DrawLineWidth((Vector2)cmd.args[0], (Vector2)cmd.args[1], (float)cmd.args[2], (Color)cmd.args[3], (bool)cmd.args[4]);
                    }
                    else { throw new Exception("未定义 参数"); }
                    break;
                case GLCmdType.DrawQuadOrtho:
                    if (ArgType<Vector2, Vector2, Vector2, Vector2, Color>(cmd))
                    {
                        GLUI._DrawQuads((Vector2)cmd.args[0], (Vector2)cmd.args[1], (Vector2)cmd.args[2], (Vector2)cmd.args[3], (Color)cmd.args[4]);
                    }
                    else { throw new Exception("未定义 参数"); }
                    break;
                case GLCmdType.DrawTexOrtho:
                    if (ArgType<Texture2D, Color, Vector2, Vector2, Vector2, Vector2>(cmd))
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