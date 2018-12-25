using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public static class IMUI
    {
        public static GUIStyle fontStyle = new GUIStyle();
        public static int fontSize = 24; // reference size设计时的字体大小
        public static IMCmd Cmd(IMCmdType type, params object[] args)
        {
            var cmd = new IMCmd();
            cmd.type = type;
            cmd.args = args;
            return cmd;
        }
        /// <summary>
        /// LB
        /// </summary>
        public static Vector2 DrawText(string content, Vector2 pos, Vector2 pivot)
        {
            UI.AddCommand(Cmd(IMCmdType.DrawText, content, pos, pivot));
            return fontStyle.CalcSize(new GUIContent(content));
        }
        /// <summary>
        /// LB
        /// </summary>
        public static Vector2 DrawText(string content, Vector2 pos)
        {            
            UI.AddCommand(Cmd(IMCmdType.DrawText, content, pos));
            return fontStyle.CalcSize(new GUIContent(content));
        }
        /// <summary>
        /// return Reference Size
        /// </summary>
        public static Vector2 CalSize(string content)
        {
            var n = new GUIStyle(fontStyle);
            n.fontSize = fontSize; // 设计时的大小
            Vector2 size = n.CalcSize(new GUIContent(content));
            return size;
        }
        // ref pos
        public static void _DrawTextRef(string content, Vector2 pos)
        {
            pos *= UI.facterToRealPixel;
            fontStyle.fontSize = Mathf.RoundToInt(fontSize * UI.facterToRealPixel);
            Vector2 size = fontStyle.CalcSize(new GUIContent(content)); // 计算对应样式的字符尺寸  
            GUI.Label(new UnityEngine.Rect(pos, size), content, fontStyle);
        }
        public static void _DrawTextRef(string content, Vector2 pos, Vector2 pivot)
        {
            pos *= UI.facterToRealPixel;
            fontStyle.fontSize = Mathf.RoundToInt(fontSize * UI.facterToRealPixel);
            Vector2 size = fontStyle.CalcSize(new GUIContent(content));
            GUI.Label(new UnityEngine.Rect(pos - Vector2.Scale(size, pivot), size), content, fontStyle);
        }
        // screen pos
        public static void _DrawText(string content, Vector2 pos)
        {
            pos = pos.f_sub_y(Screen.height);
            fontStyle.fontSize = Mathf.RoundToInt(fontSize * UI.facterToRealPixel);
            Vector2 size = fontStyle.CalcSize(new GUIContent(content)); // 计算对应样式的字符尺寸  
            GUI.Label(new UnityEngine.Rect(pos, size), content, fontStyle);
        }
        public static void _DrawText(string content, Vector2 pos, Vector2 pivot)
        {
            pos = pos.f_sub_y(Screen.height);
            fontStyle.fontSize = Mathf.RoundToInt(fontSize * UI.facterToRealPixel);
            Vector2 size = fontStyle.CalcSize(new GUIContent(content));
            GUI.Label(new UnityEngine.Rect(pos - Vector2.Scale(size, pivot), size), content, fontStyle);
        }
    }
}