using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa.UI_;

public class UILoading : MonoBehaviour
{
    public float pointSize = 15;
    public Color color = Color.white;
    public Vector2 posStart = new Vector2(0, 60);
    public int count = 7;
    public float angle = 18;
    public float sizeDelta = -2;
    public float interval = 0.2f;
    Vector2 _posStart;
    [Esa.Button]
    private void OnEnable()
    {
        _posStart = posStart;
        this.AddInput(Input, 0, false);
    }
    float timer;
    void Input()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            _posStart = Quaternion.AngleAxis(-angle, Vector3.forward) * _posStart;
        }
        this.BeginOrtho();
        var posRT = UI.AbsRefPos(this);
        var pos = _posStart;
        var s = pointSize;
        for (int i = 0; i < count; i++)
        {
            s += sizeDelta;
            pos = Quaternion.AngleAxis(angle, Vector3.forward) * pos;
            UITool.DrawSquare(posRT + pos, s, color, true);
        }
    }
}
