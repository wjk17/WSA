using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.BT
{
    [Serializable]
    public class Selector : Action
    {
        public Selector() : base() { }
        public Selector(PreCondition preCond) : base(preCond) { }
        public Selector(PreCondition preCond, Execute execute) : base(preCond, execute) { }
        public Selector(PreCondition preCond, Enter enter, Execute execute) :
            base(preCond, enter, execute)
        { }
        public int ExecuteIfTrue()
        {
            if (PreCondition()) return Execute();
            return 0;
        }
        public override int Execute()
        {
            foreach (var child in childs)
            {
                var action = child as Action;
                if (action.PreCondition())
                {
                    action.Enter(); // 进入状态事件
                    var result = action.Execute();
                    if (result == 2) agent.running = action;
                    return result;
                }
            }
            return 0;
        }
    }
}