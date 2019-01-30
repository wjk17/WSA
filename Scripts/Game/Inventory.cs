using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.UI_
{
    public class Inventory : Singleton<Inventory>
    {
        // 获得物品
        public int[] itemsID;
        // 经验
        public int exp;
        // 金钱
        public int money;
        void Start()
        {
            this.AddInput(Input, 0);
            lineSpace = GLUI.fontSize + 5;
        }
        Vector2 pos;
        int lineSpace;

        void Input()
        {
            this.BeginOrtho();
            pos = Vector2.zero;

            DrawLine("获得物品：");
            if (itemsID.Length == 0)
            {
                DrawLineT2("无");
            }
            else
            {
                foreach (var item in itemsID)
                {
                    var name = Items.GetItem(item).name;
                    DrawLineT2(name);
                }
            }
            DrawLine("金钱：");
            DrawLineT2(money.ToString());

            DrawLine("经验：");
            for (int i = 0; i < Battle.allys.Count; i++)
            {
                DrawLineT2(Battle.allys[i].P.charName + "    " + Battle.expEarns[i].ToString());
            }
        }
        void DrawLineT2(string s)
        {
            DrawLine("    " + s);
        }
        void DrawLine(string s)
        {
            GLUI.DrawString(s, pos, Vector2.up);
            pos.y += lineSpace;
        }
    }
}