using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
using Esa.UI_;
using System;
using UnityEngine.SceneManagement;

public class UITitleMenu : Singleton<UITitleMenu> {
    public Button_Row buttons;
	// Use this for initialization
	void Awake () {
        buttons = GetComponent<Button_Row>();
        buttons.onClick = OnClick;
    }

    private void OnClick(int i)
    {
        if (i == 0)
        {
            UISL.I.NewGameUI();
            buttons.gameObject.SetActive(false);
        }
        else if (i == 1)
        {
            UISL.I.LoadGameUI();
            buttons.gameObject.SetActive(false);
        }
        else if (i == 2)
            Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
