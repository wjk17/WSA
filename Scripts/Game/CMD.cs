using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    [Serializable]
    public class CMD
    {
        public CMDType type;
        public object[] args;
        public object[] result;
        public CMD(CMDType type)
        {
            this.type = type;
        }
        public CMD(CMDType type, params object[] args)
        {
            this.type = type;
            this.args = args;
        }
        public CMD Clone()
        {
            return new CMD(type, args);
        }
    }
    public enum CMDType
    {
        calculate, // 计算        
        flee, // 战斗中逃跑
        setProp, // 修改属性
        getProp, // 获取属性

        setPropT, // 修改目标属性
        getPropT, // 获取目标属性

        ifThen, // 条件执行

        print, // 打印消息
    }
    public enum OP
    {
        plus,
        minus,
        multi,
        divide,

        Compare,
        Greater,
        Less,
        Equal,
        GEqual,
        LEqual,

        Random,
    }
    public enum PropType
    {
        lvl,
        hp,
        mp,
        str,
        sta,
        atk,
    }
    public enum CharSelect
    {
        Owner,
        Target
    }
}