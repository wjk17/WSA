using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.BT
{
    public delegate bool PreCondition();
    public delegate void Enter();
    [Serializable]
    public class Action : Node
    {
        public PreCondition preCond;
        public Enter enter;
        [Header("状态")]
        public bool _preCond;
        public Action() : base() { }
        public Action(PreCondition preCond)
        {
            this.preCond = preCond;
        }
        public Action(PreCondition preCond, Execute execute)
        {
            this.preCond = preCond;
            this.execute = execute;
        }
        public Action(PreCondition preCond, Enter enter, Execute execute)
        {
            this.preCond = preCond;
            this.enter = enter;
            this.execute = execute;
        }
        public virtual void Enter()
        {
            if (enter != null) enter();
        }
        public virtual bool PreCondition()
        {
            _preCond = preCond();
            return _preCond;
        }
    }
}