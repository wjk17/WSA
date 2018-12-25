using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;
using Esa.UI;
public class UIBlock : Singleton<UIBlock>
{
    public override void _Awake()
    {
        base._Awake();
        this.AddInput(-999);
        var btn = this.GetComChild<Button>();
        btn.onClick.AddListener(() =>
        {
            BlockUI(false);
        });
    }
    public void BlockUI(bool block)
    {
        foreach (var mouse in TransTool.GetComsScene<MouseEventWrapper>())
        {
            mouse.enabled = !block;
        }
        foreach (var grid in TransTool.GetComsScene<UIGrid>())
        {
            grid.enabled = !block;
        }
        foreach (var row in TransTool.GetComsScene<Button_Row>())
        {
            row.enabled = !block;
        }
        foreach (var button in TransTool.GetComsScene<Button>())
        {
            if (button != this.GetComChild<Button>())
                button.enabled = !block;
        }
        gameObject.SetActive(block);
    }
}
