using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [Serializable]
    public class Skill
    {
        public string name; // 技能名字
        public string id;
        public List<CMD> cmds;
        public Skill(string name, string id)
        {
            this.name = name;
            this.id = id;
            cmds = new List<CMD>();
        }
        public void AddCMD(CMD cmd)
        {
            cmds.Add(cmd);
        }
        public void AddCMD(CMDType type, params object[] args)
        {
            cmds.Add(new CMD(type, args));
        }
        public void Cast()
        {
            foreach (var cmd in cmds)
            {
                CMDParser.Execute(cmd);
            }
        }
        public void Cast(Char owner, params Char[] targets)
        {
            List<Char> list = new List<Char>();
            list.Add(owner);
            list.AddRange(targets);
            CMDParser.chars = list;
            foreach (var cmd in cmds)
            {
                CMDParser.Execute(cmd);
            }
        }
    }
}