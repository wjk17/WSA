using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class _GLDrawText : MonoBehaviour
{
    public Material tex_mat;
    public Texture texture;
    public Vector2[] vs;
    void OnPostRender()
    {
        DrawTex(texture, vs);
    }
    /// <summary>
    /// Clockwise Reverse
    /// </summary>
    void DrawTex(Texture texture, params Vector2[] v)
    {
        DrawTex(texture, Color.white, v);
    }
    void DrawTex(Texture texture, Color color, params Vector2[] v)
    {
        tex_mat.SetTexture("_MainTex", texture);
        tex_mat.SetPass(0);
        GL.Color(color);
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        var uvs = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0)};
        for (int i = 0; i < 4; i++)
        {
            GL.TexCoord(uvs[i]);
            GL.Vertex(v[i]);
        }
        GL.End();
    }
}
