using UnityEngine;
using System;
using System.Collections.Generic;
namespace Esa.UI
{
    public enum LINE_MODE { LINES, TRIANGLES, TETRAHEDRON };

    public static class DrawLines
    {
        private static IList<Vector3> m_corners4d = new Vector3[8];
        private static IList<Vector3> m_corners4f = new Vector3[8];
        private static IList<Vector3> m_vertices;

        private static Material m_lineMaterial;
        private static Material LineMaterial
        {
            get
            {
                if (m_lineMaterial == null)
                    m_lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
                return m_lineMaterial;
            }
        }

        private static IList<int> m_cube = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        public static void DrawVertices(LINE_MODE mode, Camera camera, Color color, IList<Vector3> vertices, IList<int> indices, Matrix4x4 localToWorld)
        {
            if (camera == null || vertices == null || indices == null) return;

            int count = vertices.Count;
            IList<Vector3> verts = Vertices(count);
            for (int i = 0; i < count; i++)
                verts[i] = localToWorld.MultiplyPoint(vertices[i]);

            switch (mode)
            {
                case LINE_MODE.LINES:
                    DrawVerticesAsLines(camera, color, verts, indices);
                    break;

                case LINE_MODE.TRIANGLES:
                    DrawVerticesAsTriangles(camera, color, verts, indices);
                    break;

                case LINE_MODE.TETRAHEDRON:
                    DrawVerticesAsTetrahedron(camera, color, verts, indices);
                    break;
            }
        }
        public static void DrawQuads(Color color, IList<Vector2> vs, Matrix4x4 m)
        {
            if (vs == null) return;

            for (int i = 0; i < vs.Count; i++)
                vs[i] = m.MultiplyPoint(vs[i]);

            DrawQuads(vs[0].x, vs[0].y, vs[1].x, vs[1].y, vs[2].x, vs[2].y, vs[3].x, vs[3].y, color);
        }
        public static void DrawQuads(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, Color color)
        {

            GL.LoadOrtho();

            LineMaterial.SetPass(0);

            GL.Begin(GL.QUADS);
            GL.Color(color);
            //y1 = UI.scaler.referenceResolution.y - y1;
            //y2 = UI.scaler.referenceResolution.y - y2;
            //y3 = UI.scaler.referenceResolution.y - y3;
            //y4 = UI.scaler.referenceResolution.y - y4;
            //GL.Vertex3(x1 / UI.scaler.referenceResolution.x, y1 / UI.scaler.referenceResolution.y, 0);
            //GL.Vertex3(x2 / UI.scaler.referenceResolution.x, y2 / UI.scaler.referenceResolution.y, 0);
            //GL.Vertex3(x3 / UI.scaler.referenceResolution.x, y3 / UI.scaler.referenceResolution.y, 0);
            //GL.Vertex3(x4 / UI.scaler.referenceResolution.x, y4 / UI.scaler.referenceResolution.y, 0);
            GL.Vertex3(x1, y1, 0);
            GL.Vertex3(x2, y2, 0);
            GL.Vertex3(x3, y3, 0);
            GL.Vertex3(x4, y4, 0);
            GL.End();
        }
        public static void DoDrawLines(Color color, IList<Vector2> vs, IList<int> indices)
        {
            if (vs == null || indices == null) return;

            GL.LoadOrtho();

            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vs.Count;

            for (int i = 0; i < indices.Count / 2; i++)
            {
                int i0 = indices[i * 2 + 0];
                int i1 = indices[i * 2 + 1];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;

                GL.Vertex(vs[i0]);
                GL.Vertex(vs[i1]);
            }

            GL.End();
        }
        public static void DoDrawLines(Color color, IList<Vector2> vs, IList<int> indices, Matrix4x4 m)
        {
            if (vs == null || indices == null) return;

            for (int i = 0; i < vs.Count; i++)
                vs[i] = m.MultiplyPoint(vs[i]);


            LineMaterial.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vs.Count;

            for (int i = 0; i < indices.Count / 2; i++)
            {
                int i0 = indices[i * 2 + 0];
                int i1 = indices[i * 2 + 1];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;

                GL.Vertex(vs[i0]);
                GL.Vertex(vs[i1]);
            }

            GL.End();
        }
        private static void DrawVerticesAsLines(Camera camera, Color color, IList<Vector3> vertices, IList<int> indices)
        {
            if (camera == null || vertices == null || indices == null) return;

            //GL.PushMatrix();

            //GL.LoadIdentity();
            //GL.MultMatrix(camera.worldToCameraMatrix);
            //GL.LoadProjectionMatrix(camera.projectionMatrix);

            GL.LoadOrtho();

            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;

            for (int i = 0; i < indices.Count / 2; i++)
            {
                int i0 = indices[i * 2 + 0];
                int i1 = indices[i * 2 + 1];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;

                GL.Vertex(vertices[i0]);
                GL.Vertex(vertices[i1]);
            }

            GL.End();

            //GL.PopMatrix();
        }

        private static void DrawVerticesAsTriangles(Camera camera, Color color, IList<Vector3> vertices, IList<int> indices)
        {
            if (camera == null || vertices == null || indices == null) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;

            for (int i = 0; i < indices.Count / 3; i++)
            {
                int i0 = indices[i * 3 + 0];
                int i1 = indices[i * 3 + 1];
                int i2 = indices[i * 3 + 2];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;

                GL.Vertex(vertices[i0]);
                GL.Vertex(vertices[i1]);

                GL.Vertex(vertices[i0]);
                GL.Vertex(vertices[i2]);

                GL.Vertex(vertices[i2]);
                GL.Vertex(vertices[i1]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private static void DrawVerticesAsTetrahedron(Camera camera, Color color, IList<Vector3> vertices, IList<int> indices)
        {
            if (camera == null || vertices == null || indices == null) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            int vertexCount = vertices.Count;

            for (int i = 0; i < indices.Count / 4; i++)
            {
                int i0 = indices[i * 4 + 0];
                int i1 = indices[i * 4 + 1];
                int i2 = indices[i * 4 + 2];
                int i3 = indices[i * 4 + 3];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;

                GL.Vertex3(vertices[i0].x, vertices[i0].y, vertices[i0].z);
                GL.Vertex3(vertices[i1].x, vertices[i1].y, vertices[i1].z);

                GL.Vertex3(vertices[i0].x, vertices[i0].y, vertices[i0].z);
                GL.Vertex3(vertices[i2].x, vertices[i2].y, vertices[i2].z);

                GL.Vertex3(vertices[i0].x, vertices[i0].y, vertices[i0].z);
                GL.Vertex3(vertices[i3].x, vertices[i3].y, vertices[i3].z);

                GL.Vertex3(vertices[i1].x, vertices[i1].y, vertices[i1].z);
                GL.Vertex3(vertices[i2].x, vertices[i2].y, vertices[i2].z);

                GL.Vertex3(vertices[i3].x, vertices[i3].y, vertices[i3].z);
                GL.Vertex3(vertices[i2].x, vertices[i2].y, vertices[i2].z);

                GL.Vertex3(vertices[i1].x, vertices[i1].y, vertices[i1].z);
                GL.Vertex3(vertices[i3].x, vertices[i3].y, vertices[i3].z);
            }

            GL.End();

            GL.PopMatrix();
        }

        public static void DrawGrid(Camera camera, Color color, Vector3 min, Vector3 max, float spacing, Matrix4x4 localToWorld)
        {
            if (camera == null) return;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            LineMaterial.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(color);

            float width = max.x - min.x;
            float depth = max.z - min.z;

            if (spacing <= 0)
                throw new ArgumentException("Spacing must be > 0");

            for (float x = 0; x <= width; x += spacing)
            {
                for (float z = 0; z <= depth; z += spacing)
                {
                    Vector4 v0 = localToWorld.MultiplyPoint(new Vector4(min.x + x, 0, min.z, 1));
                    Vector4 v1 = localToWorld.MultiplyPoint(new Vector4(min.x + x, 0, max.z, 1));

                    GL.Vertex(v0);
                    GL.Vertex(v1);

                    Vector4 v2 = localToWorld.MultiplyPoint(new Vector4(min.x, 0, min.z + z, 1));
                    Vector4 v3 = localToWorld.MultiplyPoint(new Vector4(max.x, 0, min.z + z, 1));

                    GL.Vertex(v2);
                    GL.Vertex(v3);
                }
            }

            GL.End();

            GL.PopMatrix();
        }

        private static IList<Vector3> Vertices(int count)
        {
            if (m_vertices == null || m_vertices.Count < count)
                m_vertices = new Vector3[count];

            return m_vertices;
        }
    }
}