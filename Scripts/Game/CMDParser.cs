using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    public class CMDParser : MonoBehaviour
    {
        public static List<Char> chars;
        public static List<CMD> process;
        public static void Execute(CMD cmd)
        {
            if (process.Contains(cmd)) return;
            if (cmd.type == CMDType.ifThen)
            {
                IfCMD(cmd);
                return;
            }
            var levelSeek = new List<int>(); // 每个层级当前的执行位置     
            var level = 0; // 调用层级
            CMD cmdUpper = null;
            loop:
            if (level >= levelSeek.Count)
                levelSeek.Add(0); // 创建新层级，执行位置0

            if (cmd.args == null) goto self;
            // 深度优先递归执行命令，防止溢出，将结果保存到命令自身。
            // TODO 将命令从树结构展平为列表，拷贝命令树
            for (int i = levelSeek[level]; i < cmd.args.Length; i++)
            {
                levelSeek[level]++; // 执行位置右移
                var c = cmd.args[i] as CMD;
                if (c != null)
                {
                    if (c.type == CMDType.ifThen)
                    {
                        IfCMD(c);
                    }
                    else if (IsEmbedCMD(c))
                    {
                        level++;// 下一层
                        cmdUpper = cmd;
                        cmd = c;
                        //print("goto " + level);
                        goto loop;
                    }
                    else ExecuteCMD(c); // 非嵌套命令直接执行
                }
            }
            self:
            // 已经执行完所有子命令，执行自己
            ExecuteCMD(cmd);
            if (level > 0) // 如果有父命令，返回上层
            {
                level--;
                cmd = cmdUpper;
                //print("back " + level);
                goto loop;
            }
        }
        static bool IsEmbedCMD(CMD cmd) // 是否是嵌套
        {
            foreach (var arg in cmd.args)
            {
                if (arg is CMD) return true;
            }
            return false;
        }
        public static void ExecuteCMD(CMD cmd)
        {
            if (process.Contains(cmd)) return;
            process.Add(cmd);
            switch (cmd.type)
            {
                case CMDType.getProp:
                    cmd.result = new object[] { GetProp(cmd) };
                    break;
                case CMDType.setProp:
                    SetProp(cmd);
                    break;
                case CMDType.calculate:
                    cmd.result = Calculate(cmd);
                    break;
                case CMDType.ifThen:
                    IfCMD(cmd);
                    break;
                case CMDType.flee:
                    Battle.End();
                    break;
                case CMDType.print:
                    Print(cmd);
                    break;
                default:
                    throw new Exception("undef CMDType");
            }
        }

        private static void Print(CMD cmd)
        {
            var arg = cmd.args[0];

            if (IsValue0ArgType<string>(arg))
                print(GetValue0FromArg<string>(arg));

            else if (IsValue0ArgType<int>(arg))
                print(GetValue0FromArg<int>(arg).ToString());

            else if (IsValue0ArgType<bool>(arg))
                print(GetValue0FromArg<bool>(arg).ToString());
        }

        private static void IfCMD(CMD cmd)
        {
            if (cmd.args[0] is CMD)
            {
                Execute((CMD)cmd.args[0]);
            }
            if (cmd.args.Length == 2)
            {
                if (GetValue0FromArg<bool>(cmd.args[0]))
                {
                    Execute((CMD)cmd.args[1]);
                }
            }
            else if (cmd.args.Length == 3)
            {
                if (GetValue0FromArg<bool>(cmd.args[0]))
                {
                    Execute((CMD)cmd.args[1]);
                }
                else
                {
                    Execute((CMD)cmd.args[2]);
                }
            }
            else throw new Exception("undef IfCMD");
        }
        private static object[] Calculate(CMD cmd)
        {
            var op = (OP)cmd.args[0];
            int v1 = 0, v2 = 0, v3 = 0;
            if (cmd.args.Length > 1) v1 = GetValue0FromArg<int>(cmd.args[1]);
            if (cmd.args.Length > 2) v2 = GetValue0FromArg<int>(cmd.args[2]);
            if (cmd.args.Length > 3) v3 = GetValue0FromArg<int>(cmd.args[3]);
            switch (op)
            {
                case OP.plus: return new object[] { v1 + v2 };
                case OP.minus: return new object[] { v1 - v2 };
                case OP.multi: return new object[] { v1 * v2 };
                case OP.divide: return new object[] { v1 / v2 };
                case OP.Random:
                    var v = Random.IntValue(v1);
                    //print("Random" + v);
                    return new object[] { v };
                case OP.Greater: return new object[] { v1 > v2 };
                case OP.GEqual:
                    //print("GEqual: " + v1 + " " + v2);
                    return new object[] { v1 >= v2 };
                case OP.Less:
                case OP.Equal:
                case OP.Compare:
                default:
                    throw new Exception("undef Operator");
            }
        }
        public bool ArgType<T1, T2, T3>(CMD cmd)
        {
            if (cmd.args.Length != 3) return false;
            if (!(cmd.args[0] is T1)) return false;
            if (!(cmd.args[1] is T2)) return false;
            if (!(cmd.args[2] is T3)) return false;
            return true;
        }
        public static bool IsValue0ArgType<T>(object arg)
        {
            if (arg is CMD)
                return ((CMD)arg).result[0] is T;
            else
                return arg is T;
        }
        public static T GetValue0FromArg<T>(object arg)
        {
            if (arg is CMD)
                return (T)((CMD)arg).result[0];
            else
                return (T)arg;
        }

        private static int GetProp(CMD cmd)
        {
            var P = chars[GetValue0FromArg<int>(cmd.args[0])].P;
            switch ((PropType)cmd.args[1])
            {
                case PropType.lvl: return P.lvl;
                case PropType.hp: return P.hp;
                case PropType.mp: return P.mp;
                case PropType.str: return P.strength;
                case PropType.sta: return P.stamina;
                case PropType.atk: return P.attack;
                default:
                    throw new Exception("undef GetProp");
            }
        }
        private static void SetProp(CMD cmd) // P, proptype, value
        {
            var P = chars[GetValue0FromArg<int>(cmd.args[0])].P;
            var v = GetValue0FromArg<int>(cmd.args[2]);

            switch ((PropType)cmd.args[1])
            {
                case PropType.lvl: P.lvl = v; break;
                case PropType.hp: P.hp = v; break;
                case PropType.mp: P.mp = v; break;
                case PropType.str: P.strength = v; break;
                case PropType.sta: P.stamina = v; break;
                case PropType.atk: P.attack = v; break;
                default:
                    throw new Exception("undef GetProp");
            }
        }
    }
}