using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
public class UINaming : Singleton<UINaming>
{
    public Button confirm;
    public Button cancel;
    public InputField charName;
    public int minLength = 1;
    public override void _Start()
    {
        confirm = GetComponentsInChildren<Button>()[0];
        cancel = GetComponentsInChildren<Button>()[1];
        cancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            UITitleMenu.I.buttons.gameObject.SetActive(true);
        });
        charName = GetComponentInChildren<InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
