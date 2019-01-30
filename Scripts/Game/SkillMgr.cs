using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public class SkillMgr : Singleton<SkillMgr>
    {
        public List<Skill> skills;
        // Use this for initialization
        void Start()
        {
            InitSkill();
        }
        void AddSkill(Skill sk)
        {
            skills.Add(sk);
        }
        public static Skill GetSkill(string id)
        {
            return I._GetSkill(id);
        }
        public Skill _GetSkill(string id)
        {
            foreach (var sk in skills)
            {
                if (sk.id == id) return sk;
            }
            return null;
        }
        CMD Cal(params object[] args)
        {
            return new CMD(CMDType.calculate, args);
        }
        CMD Print(params object[] args)
        {
            return new CMD(CMDType.print, args);
        }
        CMD If(params object[] args)
        {
            return new CMD(CMDType.ifThen, args);
        }
        public void InitSkill()
        {
            var attack = new Skill("攻击", "attack");
            var damage = new CMD(CMDType.getProp, PropType.atk);
            var hp = new CMD(CMDType.getProp, PropType.hp);
            var hp2 = new CMD(CMDType.calculate, OP.minus, hp, damage);
            attack.AddCMD(CMDType.setProp, CharSelect.Target, PropType.hp, hp2); // 普通攻击
            AddSkill(attack);

            var flee = new Skill("逃跑", "flee");
            var fl = new CMD(CMDType.flee);
            var random = Cal(OP.Random, 100);  // 成功概率（命中率）百分比
            var success = Cal(OP.GEqual, 50, random);
            var s2 = Cal(OP.GEqual, 50, random.Clone());


            flee.AddCMD(If(success, If(s2, Print("大成功"), Print("小成功")), Print("失败")));
            //flee.AddCMD(CMDType.ifThen, success, fl);
            AddSkill(flee);


            var doubleHit = new Skill("二连击", "doubleHit");
            // 计算魔法消耗，如果够蓝则释放技能
            var mpExpendAmount = 35;
            var mpExpend = new CMD(CMDType.setProp, PropType.mp, OP.minus, mpExpendAmount);
            doubleHit.AddCMD(mpExpend);
            var mp = new CMD(CMDType.getProp, PropType.mp);
            var enoughMp = new CMD(CMDType.calculate, OP.GEqual, mp, mpExpendAmount);
            var mpExpendIf = new CMD(CMDType.ifThen, enoughMp, doubleHit);

            var atk = new CMD(CMDType.getProp, PropType.atk);
            var dmg = new CMD(CMDType.calculate, OP.multi, atk, 0.85f);
            doubleHit.AddCMD(CMDType.setProp, CharSelect.Target, PropType.hp, OP.minus, dmg);
            AddSkill(doubleHit);

            var backStab = new Skill("背击", "backStab");
            backStab.AddCMD(CMDType.setProp, PropType.mp, OP.minus, 30); // 耗蓝
            AddSkill(backStab);
        }
        [Button]
        void Print()
        {
            foreach (var sk in skills)
            {
                print(sk.name);
            }
        }
    }
}