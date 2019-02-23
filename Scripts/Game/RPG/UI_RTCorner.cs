using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa.UI_;
using Esa;
using System;

public class UI_RTCorner : MonoBehaviour
{
    public UIGrid grid;
    public Vector2 offset = new Vector2(-10,-10);

    // Use this for initialization
    void Start()
    {
        grid = GetComponent<UIGrid>();
        grid.onClick = OnClick;
    }
    public float zoomSensitive = 0.3f;
    private void OnClick(int i)
    {
        switch (i)
        {
            case 0:
                // mute
                AudioMgr.I.volumeTotalOn = !AudioMgr.I.volumeTotalOn;
                break;
            case 1:
                // Zoom In
                UIMap.I.viewScale += zoomSensitive;
                break;
            case 2:
                // Zoom Out
                UIMap.I.viewScale -= zoomSensitive;
                break;
            case 3:
                UIPop_InfoMenu.I.SetUIPos(GetPos(grid.rects[i]));
                UIPop_InfoMenu.I.ToggleActive();
                break;
            case 4:
                // Setting
                UISettingMenu.I.ToggleActive();
                break;
            default:
                break;
        }
    }

    private Vector2 GetPos(Esa.Rect rect)
    {
        return rect.pos - rect.size * new Vector2(-0.5f, 0.5f) + offset;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
