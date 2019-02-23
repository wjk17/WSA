using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
using Esa.UI_;
using System;

public class UI_Desc : Singleton<UI_Desc>
{
    public RectTransform BgRT;
    public RectTransform titleRT;
    public RectTransform descRT;
    public RectTransform descBgRT;
    public new string name;
    public string desc;
    public override void _Start()
    {
        UI.I.earlyUpdate = () => I.gameObject.SetActive(false);
        this.AddInput(_Input, 2);
        this.DestroyImages();
    }
    void _Input()
    {
        var rtBg = new RectTrans(BgRT);
        var rtTitle = new RectTrans(titleRT);
        var rtDesc = new RectTrans(descRT);
        var rtDescBg = new RectTrans(descBgRT);

        this.BeginOrtho();
        this.DrawBG();
        GLUI.DrawString(rtTitle.cornerLB, name);
        GLUI.DrawString(rtDesc.cornerLT - 24.Y(), desc);
    }
}
