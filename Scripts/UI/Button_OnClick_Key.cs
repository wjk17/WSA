using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public class Button_OnClick_Key : MonoBehaviour
    {
        public KeyCode keyCode;
        void Update()
        {
            if (Events.KeyDown(keyCode))
<<<<<<< HEAD
                GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
=======
                GetComponent<Button>().onClick.Invoke();
>>>>>>> 36ecf3a9dfc01741cc93e9b0c92d2ca525d75f9d
        }
    }
}