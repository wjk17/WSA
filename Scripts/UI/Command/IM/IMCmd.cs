﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI_
{
    public enum IMCmdType
    {
        DrawText,
    }
    [Serializable]
    public class IMCmd : Cmd
    {
        public IMCmdType type;
    }
}
