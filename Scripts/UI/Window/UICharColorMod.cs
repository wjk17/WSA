using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using Esa.UI_;
public class UICharColorMod : Singleton<UICharColorMod>
{
    UIColorPicker[] pickers;
    Button_Tab obj;
    public Material[] mats;
    public Material mat;
    public int idx = -1;
    public string nameSel;
    public ColorPalette[] palettes;
    string[] ns;
    // Use this for initialization
    void Start()
    {
        ns = new string[] { "_BaseColor", "_1st_ShadeColor", "_2nd_ShadeColor" };
        pickers = GetComponentsInChildren<UIColorPicker>(true);
        palettes = GetComponentsInChildren<ColorPalette>(true);
        obj = GetComponentInChildren<Button_Tab>(true);
        idx = obj.idx;
        if (idx >= 0) LoadMat();
        obj.onClick = (i) =>
        {
            idx = i;
            LoadMat();
        };
    }
    public void LoadMat()
    {
        nameSel = mats[idx].name;
        int i = 0;
        foreach (var n in ns)
        {
            pickers[i].palette = palettes[idx].list.colors;
            pickers[i].tips = palettes[idx].list.names;
            pickers[i++].color = mats[idx].GetColor(n);
        }
    }
    void Update()
    {
        if (idx < 0) return;
        int i = 0;
        foreach (var n in ns)
        {
            mats[idx].SetColor(n, pickers[i++].color);
        }
        if (idx == 2) Copy(mats[2], mat);
    }
    void Copy(Material from, Material to)
    {
        foreach (var n in ns)
        {
            to.SetColor(n, from.GetColor(n));
        }
    }
}
