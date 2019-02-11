using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public static partial class GLUI
    {
        public static Font font;
        public static int fontSize;
        public static Color fontColor = Color.white;
        public static void DrawString(string str, Vector2 pos)
        {
            DrawString(pos, str, fontSize);
        }
        public static void DrawString(string str, Vector2 pos, int fontSize)
        {
            DrawString(pos, str, fontSize, Vector2.zero);
        }
        public static void DrawString(string str, Vector2 pos, Vector2 pivot)
        {
            DrawString(pos, str, fontSize, pivot);
        }
        public static void DrawString(string str, Vector2 pos, int fontSize, Vector2 pivot)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawString, pos, str, fontSize, pivot));
        }
        public static void DrawString(Vector2 pos, string str)
        {
            DrawString(pos, str, fontSize);
        }
        public static void DrawString(Vector2 pos, string str, int fontSize)
        {
            DrawString(pos, str, fontSize, Vector2.zero);
        }
        public static void DrawString(Vector2 pos, string str, Vector2 pivot)
        {
            DrawString(pos, str, fontSize, pivot);
        }
        public static void DrawString(Vector2 pos, string str, int fontSize, Vector2 pivot)
        {
            UI.AddCommand(Cmd(commandOrder, GLCmdType.DrawString, pos, str, fontSize, pivot));
        }
        public static void _DrawString(Vector2 pos, string str, int fontSize, Vector2 pivot)
        {
            Vector2[] vs;
            Vector2[] uv;
            RebuildMesh(str, out vs, out uv, fontSize, pivot);
            font.material.SetPass(0);
            GL.LoadOrtho();
            GL.Color(fontColor);
            var factor = Vector2.one / UI.scalerRefRes;
            for (int i = 0; i < vs.Length; i += 4)
            {
                GL.Begin(GL.QUADS);
                for (int j = 0; j < 4; j++)
                {
                    GL.TexCoord(uv[i + j]);
                    GL.Vertex((pos + vs[i + j]) * factor);
                }
                GL.End();
            }
        }
        static void RebuildMesh(string str, out Vector2[] vs, out Vector2[] uv, int fontSize, Vector2 pivot)
        {
            // Generate a mesh for the characters we want to print.
            vs = new Vector2[str.Length * 4];
            uv = new Vector2[str.Length * 4];

            if (str.Length == 0) return;
            // Request characters.
            font.RequestCharactersInTexture(str, fontSize);

            var pos = Vector2.zero;
            var size = Vector2.zero;
            for (int i = 0; i < str.Length; i++)
            {
                // Get character rendering information from the font
                CharacterInfo ch;
                font.GetCharacterInfo(str[i], out ch, fontSize);

                size = pos + new Vector2(ch.maxX, ch.maxY);

                vs[4 * i + 0] = pos + new Vector2(ch.minX, ch.maxY);
                vs[4 * i + 1] = pos + new Vector2(ch.maxX, ch.maxY);
                vs[4 * i + 2] = pos + new Vector2(ch.maxX, ch.minY);
                vs[4 * i + 3] = pos + new Vector2(ch.minX, ch.minY);

                uv[4 * i + 0] = ch.uvTopLeft;
                uv[4 * i + 1] = ch.uvTopRight;
                uv[4 * i + 2] = ch.uvBottomRight;
                uv[4 * i + 3] = ch.uvBottomLeft;

                // Advance character position
                pos += new Vector2(ch.advance, 0);
            }
            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] -= size * pivot;
            }
        }
    }
}