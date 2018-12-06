using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectTool
{
    public static void SetUIFieldByName(this object owner, Transform trans, System.Type[] ts)
    {
        foreach (var field in owner.GetType().GetFields())
        {
            if (ts.Contains(field.FieldType))
            {
                var t = trans.Find(field.Name.BigCamel());
                if (t != null)
                {
                    field.SetValue(owner, t.GetComponent(field.FieldType));
                }
                else Debug.Log("not found ui: " + field.Name);
            }
        }
    }
}
