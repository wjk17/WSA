using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class BattleMenu : Singleton<BattleMenu>
    {
        UIGrid grid;
        public int action;
        void Start()
        {
            this.AddInput(_Input, -2, false);
            this.DestroyImages();
            grid = GetComponentInChildren<UIGrid>(true);
            grid.onClick = BattleAction;
        }
        public void BattleAction(int i)
        {
            action = i;
            if (i == 0) // 攻击
            {
                if (Battle.allys.Count > 1)
                {
                    I.Disactive();
                    SkillPanel.I.Disactive();
                    SelectPanel.I.Active();
                }
                //else CharCtrl.I.Attack();
            }
            else if (i == 1) // 物品
            {
                //CharCtrl.I.Healing();
            }
            else if (i == 2)
            {
                SkillPanel.I.ToggleActive();
            }
            else if (i == 3) // 逃跑
            {
                SkillMgr.GetSkill("flee").Cast();
            }
            I.Active();
        }
        void _Input()
        {
            this.BeginOrtho();
            this.DrawBG();
        }
    }
}