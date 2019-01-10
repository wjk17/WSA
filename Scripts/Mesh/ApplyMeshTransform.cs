using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Esa
{
    public class ApplyMeshTransform : MonoBehaviour
    {
        private MeshFilter mf
        {
            get { return GetComponent<MeshFilter>(); }
        }
        private Mesh mesh
        {
            get { return mf == null ? null : mf.sharedMesh; }
            set { if (mf != null) mf.sharedMesh = value; }
        }
        public float y = 0.5f;
        public string defaultPath = @"Assets\Meshs";
        public bool copyMesh = true;
        [Button]
        void Copy()
        {
            mesh = Instantiate(mesh);
        }
        [Button]
        void Apply()
        {
            if (mesh != null) mf.ApplyScale(copyMesh);
        }
#if UNITY_EDITOR
        [Button]
        void SaveAsset()
        {
            string filePath = EditorUtility.SaveFilePanelInProject(
                "Save Procedural Mesh", mesh.name, "asset", "select a location to save", defaultPath);
            if (filePath.Length > 0)
                AssetDatabase.CreateAsset(mesh, filePath);
        }
#endif
        [Button]
        void SetYTo()
        {
            if (mesh != null) mf.SetYTo(y, copyMesh);
        }
        [Button]
        void SetYToOne()
        {
            if (mesh != null) mf.SetYToOne(copyMesh);
        }
    }
}