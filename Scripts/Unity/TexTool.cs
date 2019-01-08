using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{

    public static class TexTool
    {
        public static void SetPixels(this Texture2D tex, Vector2Int pos, Vector2Int size, Color[] colors)
        {
            tex.SetPixels(pos.x, pos.y, size.x, size.y, colors);
        }
        // TODO 加一个可以获取尺寸超过贴图尺寸的像素组，用指定颜色填充
        public static Color[] GetPixels(this Texture2D tex, Vector2Int pos, Vector2Int size)
        {
            return tex.GetPixels(pos.x, pos.y, size.x, size.y);
        }
        public static Vector2Int Size(this Texture2D tex)
        {
            return new Vector2Int(tex.width, tex.height);
        }
    }
}