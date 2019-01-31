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
        private int charSel;

        void Start()
        {
            this.AddInput(_Input, -2, false);
            this.DestroyImages();
            grid = GetComponentInChildren<UIGrid>(true);
            grid.onClick = BattleAction;
        }
        void Cast(string str)
        {
            SkillMgr.GetSkill(str).Cast();
        }
        void Cast(string str, Char owner, params Char[] targets)
        {
            SkillMgr.GetSkill(str).Cast(owner, targets);
        }
        public void BattleAction(int i)
        {
            action = i;
            if (i == 0) // 攻击
            {
                if (Battle.enemys.Count > 1)
                {
                    SelectPanel.I.Active();
                    I.Disactive();
                }
                else
                {
                    Cast("attack", CharCtrl.I.C, Battle.enemy);
                    I.Disactive();
                }
                SkillPanel.I.Disactive();
            }
            else if (i == 1) // 物品
            {

            }
            else if (i == 2) // 技能
            {
                SkillPanel.I.ToggleActive();
            }
            else if (i == 3) // 逃跑
            {
                Cast("flee");
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