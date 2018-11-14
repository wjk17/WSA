using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Button_OnClick_Key : MonoBehaviour
{
    public KeyCode keyCode;
    void Update()
    {
        if (Events.KeyDown(keyCode))
            GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }
}
