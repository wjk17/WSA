using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using Esa.UI_;

public class UISettingMenu : Singleton<UISettingMenu>
{
    void Start()
    {
        this.AddInput(Input, 0, true);
        this.DestroyImages();
    }
    void Input()
    {
        this.BeginOrtho();
        this.DrawBG();
    }
}
