using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SetScaleUniform))]
[CanEditMultipleObjects]
public class SetScaleUniformEditor : E_ShowButtons<SetScaleUniform>
{

}
[ExecuteInEditMode]
#endif
public class SetScaleUniform : MonoBehaviour
{
    public float scaleUniform = 1f;
    public bool updateImme = true;
    private void Reset()
    {
        scaleUniform = transform.localScale.x;
    }
    [ShowButton]
    void SetScale()
    {
        transform.localScale = Vector3.one * scaleUniform;
    }
    private void Update()
    {
        SetScale();
    }
}
