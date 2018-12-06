using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Esa
{
    public class ButtonRowAttribute : Attribute
    {
        public ButtonRowAttribute() { }
        public ButtonRowAttribute(params string[] names) { this.names = names; }
        public string[] names;
    }
}
