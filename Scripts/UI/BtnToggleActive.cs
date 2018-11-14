﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnToggleActive : MonoBehaviour
{
    public bool startActive = true;
    public GameObject[] targets;
    void Start()
    {
        foreach (var target in targets)
        {
            target.SetActive(startActive);
        }
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(this.OnClick);
    }
    void OnClick()
    {
        foreach (var target in targets)
        {
            target.SetActive(!target.activeSelf);
        }
    }
}
