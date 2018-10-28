using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MatPropType
{
    Color,
    Int,
    Float,
    Vector,
    Texture,
}
[ExecuteInEditMode]
public class CopyMatProps : MonoBehaviour
{
    public Material from;
    public Material[] to;
    public string[] propNames;
    public MatPropType type;
    public bool isOn = true;
    void Update()
    {
        if (!isOn) return;
        foreach (var prop in propNames)
        {
            if (type == MatPropType.Color)
            {
                var color = from.GetColor(prop);
                foreach (var t in to)
                {
                    t.SetColor(prop, color);
                }
            }
            else if (type == MatPropType.Float)
            {
                var f = from.GetFloat(prop);
                foreach (var t in to)
                {
                    t.SetFloat(prop, f);
                }
            }
        }
    }
}
