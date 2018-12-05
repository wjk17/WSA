using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.BT
{
    public delegate bool PreCondition();
    public delegate void Enter();
    public delegate void Exit();
    [Serializable]
    public class Action : Node
    {
        public static PreCondition TrueCondition
        {
            get { return trueCondition; }
        }
        static PreCondition trueCondition = () => true;
        static PreCondition falseCondition = () => false;

        public PreCondition preCond = trueCondition;
        public Enter enter;
        public Exit exit;
        [Header("状态")]
        public bool _preCond;

        public Action() : base() { }
        public Action(PreCondition preCond)
        {
            this.preCond = preCond;
        }
        public Action(Execute execute)
        {
            this.execute = execute;
        }
        public Action(Enter enter, Execute execute)
        {
            this.enter = enter;
            this.execute = execute;
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
        public Action(PreCondition preCond, Enter enter, Execute execute,Exit exit)
        {
            this.preCond = preCond;
            this.enter = enter;
            this.execute = execute;
            this.exit = exit;
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