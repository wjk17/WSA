using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using UI_;
    public class ChordPlayer : MonoBehaviour
    {
        //public 
        void Start()
        {
            this.AddInput(Input, 2, false);

        }
        void Input()
        {
            this.BeginOrtho(-2);
            GLUI.SetFontColor(Color.black);
            this.Draw(Palette.L8, true);
            this.Draw(Color.black, false);
        }
    }
}