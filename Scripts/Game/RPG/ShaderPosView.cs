using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    [ExecuteInEditMode]
    public class ShaderPosView : MonoBehaviour
    {
        public string propName;
        public string propName2;
        public int id;
        public int id2;
        public Material mat;
        public GameObject marker;
        public float dist;
        public float osX_L;
        public float osX_R;
        public float osXFactor;
        public Transform osT;
        [Button]
        void SetProp()
        {
            mat = this.GetMat();
            id = Shader.PropertyToID(propName);
            id2 = Shader.PropertyToID(propName2);
        }
        void Update()
        {
            if (mat == null) return;
            var pos = mat.GetVector(id);
            var dist = mat.GetFloat(id2);
            var forward = transform.parent.TransformDirection(transform.parent.forward);
            var camDir = Camera.main.transform.TransformDirection(Camera.main.transform.forward);
            //mat.SetVector("_RedLineForward", forward);
            //mat.SetVector("_CameraForward", camDir);
            osX_L = Camera.main.WorldToViewportPoint(osT.position).x;
            osX_R = Camera.main.WorldToViewportPoint(osT.position.MirrorX()).x;
            //var x = osX + Camera.main.WorldToViewportPoint(osT.position).x;
            mat.SetFloat("_osU_L", osX_L * osXFactor);
            mat.SetFloat("_osU_R", osX_R * osXFactor);
            marker.SetPos(pos);
            marker.transform.localScale = Vector3.one * dist;

        }
    }
}