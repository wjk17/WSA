using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Esa.UI_
{
    [Serializable]
    public class IMHandler : CmdHandler
    {
        public IMHandler() : base() { }
        public IMHandler(object owner) : base(owner) { }
        public override void ExecuteCommand(Cmd command)
        {
            var cmd = command as IMCmd;
            switch (cmd.type)
            {
                case IMCmdType.DrawText:
                    if (ArgType<string, Vector2, Color>(cmd))
                        IMUI._DrawText((string)cmd.args[0], (Vector2)cmd.args[1], (Color)cmd.args[2]);
                    else if (ArgType<string, Vector2, Vector2, Color>(cmd))
                        IMUI._DrawText((string)cmd.args[0], (Vector2)cmd.args[1], (Vector2)cmd.args[2], (Color)cmd.args[3]);
                    break;
                default:
                    throw null;
            }
        }
    }
}