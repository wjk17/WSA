using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
using Esa.UI_;
using System;

public class SkillPanel : Singleton<SkillPanel>
{
    public UIGrid grid;
    CharCtrl cc;
    public override void _Start()
    {
        base._Start();
        cc = TransTool.GetComScene<CharCtrl>();
        grid = GetComponentInChildren<UIGrid>(true);
        grid.Initialize();
        grid.onClick = SkillAction;
        this.AddInput(_Input, -1, false);
    }
    private void _Input()
    {
        this.BeginOrtho();
        this.DrawBG();
    }
    public int action;
    public void SkillAction(int i)
    {
        action = i;
        if (Battle.allys.Count > 1)
        {
            Battle.I.Disactive();
            I.Disactive();
            SelectPanel.I.Active();
        }
        else
        {            
            var sk = CharCtrl.I.skills[i];
            var enemy = Battle.enemy;
            sk.Cast(CharCtrl.I.C, enemy);

            if (enemy.P.hp <= 0)
            {
                CharCtrl.I.P.exp += 180;
                Battle.expEarn += 180;
                Battle.Remove(enemy);
            }
        }
    }
}
