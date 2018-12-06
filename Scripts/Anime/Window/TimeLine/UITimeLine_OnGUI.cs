using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa.UI
{
    public partial class UITimeLine
    {
        List<Command> cmds;
        private void OnGUI()
        {
            if (cmds.NotEmpty())
            {
                CommandHandler hdl = new IMUIHandler(transform as RectTransform);
                hdl.commands = cmds;
                hdl.Execute();
            }
        }
    }
}