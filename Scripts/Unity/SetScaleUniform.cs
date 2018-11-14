using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;

[ExecuteInEditMode]
public class SetScaleUniform : MonoBehaviour
{
    public float scaleUniform = 1f;
    public bool updateImme = true;
    private void Reset()
    {
        scaleUniform = transform.localScale.x;
    }
    [Button]
    void SetScale()
    {
        transform.localScale = Vector3.one * scaleUniform;
    }
    private void Update()
    {
        SetScale();
    }
}
