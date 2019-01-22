using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    using System;
    [Serializable]
    public class UnitProp
    {
        public int hp
        {
            set
            {
                _hp = Mathf.Clamp(value, 0, hpMax);
                hpText = "生命: " + _hp.ToString() + "/" + hpMax.ToString();
            }
            get { return _hp; }
        }
        int _hp;
        public int hpMax;
        public string hpText;
        public ProgBar hpBar;
        public int mp
        {
            set
            {
                _mp = Mathf.Clamp(value, 0, mpMax);
                mpText = "魔法: " + _mp.ToString() + "/" + mpMax.ToString();
            }
            get { return _mp; }
        }
        int _mp;
        public int mpMax;
        public string mpText;
        public ProgBar mpBar;


        public int agility; // 敏捷（执行动作速度）时间factor
        public float agilityStoring;
        public ProgBar agilityBar;

        public bool AgilityCheck()
        {
            agilityStoring += Time.deltaTime * agility * 0.1f;
            if (agilityStoring >= 1)
            {
                agilityStoring = 0;
                return true;
            }
            return false;
        }

        public int lvl
        {
            set { _lvl = value; lvlText = "等级: " + _lvl.ToString(); }
            get { return _lvl; }
        }
        int _lvl;
        public string lvlText;
        public int exp
        {
            set { _exp = value; lvlText = "经验: " + _exp.ToString() + "/300"; }
            get { return _exp; }
        }
        int _exp;
        public int expMax = 300;

        public string charName;

        public Vector2 agilityBarSize;
        public Vector2 hpBarSize;
        public Vector2 mpBarSize;
        public Vector2 pos = Vector2.zero;
        public UnitProp()
        {
            mpBarSize = hpBarSize = new Vector2(120, 12);
            hpBar = new ProgBar(pos, hpBarSize, Color.red);
            mpBar = new ProgBar(pos, mpBarSize, Color.blue);

            agilityBarSize = new Vector2(120, 8);
            agilityBar = new ProgBar(pos, agilityBarSize, Color.green);
        }
        public void SetPos(Vector2 _pos)
        {
            pos = _pos;
            AddBar(15, hpBar);
            AddBar(5, mpBar);
            AddBar(7, agilityBar);
            pos.y -= 5; // name text            
        }
        void AddBar(float y, ProgBar bar)
        {
            pos.y -= y;
            bar.pos = pos;
            pos -= bar.size.Y();
        }
        public void Update()
        {
            if (exp >= expMax)
            {
                exp -= expMax;
                lvl++; // 升级
                hpMax += 10;
                mpMax += 5;
            }
            hpBar.Update((float)hp / hpMax);
            mpBar.Update((float)mp / mpMax);
            agilityBar.Update(agilityStoring);
            IMUI.DrawText(charName, pos * UI.facterToRealPixel, Vectors.half2d);
        }

        public void LoadGame()
        {
            var idx = Serializer.XMLDeSerialize<SaveFileSetting>(UISL.settingPath).idxSel;
            var savefile = Serializer.XMLDeSerialize<SaveFile>(UISL.path(idx));
            Game.totalTime = savefile.totalGameTime;
            charName = savefile.name;
            hpMax = savefile.hpMax; // 先赋值max以正常显示
            hp = savefile.hp;
            mpMax = savefile.mpMax;
            mp = savefile.mp;
            agility = savefile.agility;
            lvl = savefile.lvl;
            exp = savefile.exp;
        }
        public void SaveGame()
        {
            var idx = Serializer.XMLDeSerialize<SaveFileSetting>(UISL.settingPath).idxSel;
            Serializer.XMLSerialize(new SaveFile()
            {
                totalGameTime = Game.totalTime,
                name = charName,
                hp = hp,
                hpMax = hpMax,
                mp = mp,
                mpMax = mpMax,
                agility = agility,
                lvl = lvl,
                exp = exp,
            },
            UISL.path(idx));
        }
    }
}