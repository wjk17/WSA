using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class UI_Init
{
    public static void SingleCheck(this MonoBehaviour mono, params Toggle[] toggles)
    {
        foreach (var tgl1 in toggles)
        {
            foreach (var tgl2 in toggles)
            {
                if (tgl1 != tgl2)
                {
                    tgl1.onValueChanged.AddListener(b => { if (b) tgl2.isOn = false; });
                }
            }
        }
    }
}
