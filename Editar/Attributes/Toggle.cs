using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa
{
    public class ToggleAttribute : Attribute
    {
        public ToggleAttribute() { }
        public ToggleAttribute(string name) { this.name = name; }
        public string name;
    }
}