using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
namespace Esa
{
    /// <summary>
    /// TODO Inline功能，一行显示多个拥有标签的字段，而不是像ShowButtonRow的数组形式
    /// </summary>
    /// 
    public class InlineAttribute : Attribute
    {
        public InlineAttribute() { }
        public InlineAttribute(int idx) { this.idx = idx; }
        public int idx = 0;
        public FieldInfo field;
    }
}