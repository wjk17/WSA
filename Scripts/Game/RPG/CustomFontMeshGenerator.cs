using UnityEngine;
using System.Collections;
using Esa;
using Esa.UI_;
public static class FontMesh
{

}
[ExecuteInEditMode]
public class CustomFontMeshGenerator : MonoBehaviour
{
    public Font font;
    public string str = "Hello World";
    public int fontSize = 32;
    public Vector2 pos;
    private void OnRenderObject()
    {
        DrawString(pos, str, fontSize);
    }
    public void DrawString(Vector2 pos, string str, int fontSize)
    {
        Vector2[] vs;
        Vector2[] uv;
        RebuildMesh(str, out vs, out uv, fontSize);
        font.material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Color(Color.white);
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
        GL.PopMatrix();
    }
    void RebuildMesh(string str, out Vector2[] vs, out Vector2[] uv, int fontSize)
    {
        // Request characters.
        font.RequestCharactersInTexture(str, fontSize);

        // Generate a mesh for the characters we want to print.
        vs = new Vector2[str.Length * 4];
        uv = new Vector2[str.Length * 4];
        var pos = Vector2.zero;
        for (int i = 0; i < str.Length; i++)
        {
            // Get character rendering information from the font
            CharacterInfo ch;
            font.GetCharacterInfo(str[i], out ch, fontSize);

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
    }
}