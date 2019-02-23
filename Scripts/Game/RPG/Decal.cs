using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
[ExecuteInEditMode]
public class Decal : MonoBehaviour
{
    public string propName;
    public Texture tex;
    public Material mat;
    public DepthTextureMode mode;
    [Button]
    void SetDepthMode()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
    [Button("GetMat")]
    private void Start()
    {
        mat = this.GetMat();
    }
    void GetTex()
    {
        tex = mat.GetTexture(propName);
    }
    void SetMatrix()
    {
        //mat.SetMatrix(propName, transform.localToWorldMatrix);
        mat.SetMatrix(propName, transform.worldToLocalMatrix);
    }
    void Update()
    {
        mode = Camera.main.depthTextureMode;
        SetMatrix(); 
    }
}
