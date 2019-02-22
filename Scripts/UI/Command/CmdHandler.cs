using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI_
{
    [Serializable]
    public class CmdHandler
    {
        public object owner;
        public CmdHandler() { }
        public CmdHandler(object owner) { this.owner = owner; }
        public List<Cmd> cmds = new List<Cmd>();
        public virtual void ExecuteCommand(Cmd cmd) { }
        public virtual int SortList(Cmd a, Cmd b)
        {
            if (a.order > b.order) { return 1; }
            else if (a.order < b.order) { return -1; }
            return a.insertOrder.CompareTo(b.insertOrder);
        }
        //public virtual int SortList(Cmd a, Cmd b)
        //{
        //    //_secondOrder
        //    var c = a.order.CompareTo(b.order);
        //    if (c != 0) return c;
        //    else
        //    {
        //        if ( a.secondOrder)
        //        else
        //        {
        //            return a.secondOrder.CompareTo(b.secondOrder);
        //        }

        //            c = ;
        //        c = a.insertOrder.CompareTo(b.insertOrder);
        //    }
        //    else if (a.order < b.order) { return -1; }
        //    return a.insertOrder.CompareTo(b.insertOrder);
        //}
        public void Execute()
        {
            cmds.Sort(SortList);
            UI.owner = owner; // 使执行命令时可以知道命令的owner
            foreach (var command in cmds)
            {
                ExecuteCommand(command);
            }
        }
        public bool ArgType(Cmd cmd)
        {
            return cmd.args.Length == 0;
        }
        public bool ArgType<T1>(Cmd cmd)
        {
            if (cmd.args.Length != 1) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            return true;
        }
        public bool ArgType<T1, T2>(Cmd cmd)
        {
            if (cmd.args.Length != 2) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            if (!(cmd.args[1].GetType() == typeof(T2))) return false;
            return true;
        }
        // 待做 增加模板类型支持 List<T> 以及接口 IList<T>
        public bool ArgType<T1, T2, T3>(Cmd cmd)
        {
            if (cmd.args.Length != 3) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            if (!(cmd.args[1].GetType() == typeof(T2))) return false;
            if (!(cmd.args[2].GetType() == typeof(T3))) return false;
            return true;
        }
        public bool ArgType<T1, T2, T3, T4>(Cmd cmd)
        {
            if (cmd.args.Length != 4) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            if (!(cmd.args[1].GetType() == typeof(T2))) return false;
            if (!(cmd.args[2].GetType() == typeof(T3))) return false;
            if (!(cmd.args[3].GetType() == typeof(T4))) return false;
            return true;
        }
        public bool ArgType<T1, T2, T3, T4, T5>(Cmd cmd)
        {
            if (cmd.args.Length != 5) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            if (!(cmd.args[1].GetType() == typeof(T2))) return false;
            if (!(cmd.args[2].GetType() == typeof(T3))) return false;
            if (!(cmd.args[3].GetType() == typeof(T4))) return false;
            if (!(cmd.args[4].GetType() == typeof(T5))) return false;
            return true;
        }
        public bool ArgType<T1, T2, T3, T4, T5, T6>(Cmd cmd)
        {
            if (cmd.args.Length != 6) return false;
            if (!(cmd.args[0].GetType() == typeof(T1))) return false;
            if (!(cmd.args[1].GetType() == typeof(T2))) return false;
            if (!(cmd.args[2].GetType() == typeof(T3))) return false;
            if (!(cmd.args[3].GetType() == typeof(T4))) return false;
            if (!(cmd.args[4].GetType() == typeof(T5))) return false;
            if (!(cmd.args[5].GetType() == typeof(T6))) return false;
            return true;
        }
    }
}
