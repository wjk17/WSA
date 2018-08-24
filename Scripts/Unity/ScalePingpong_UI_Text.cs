using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalePingpong_UI_Text : MonoBehaviour
{
    public float timePingpongOnce = 0.2f;
    public bool doWhenEnable = true;
    public float maxScale = 1.2f;
    int originSize;
    bool on;
    float timer;
    Text text;
    void OnEnable()
    {
        text = GetComponent<Text>();
        originSize = text.fontSize;
        if (doWhenEnable) on = true;
    }
    void Update()
    {
        if (!on) return;
        timer += Time.deltaTime;
        if (timer > timePingpongOnce)
        {
            if (timer > timePingpongOnce * 2)
            {
                text.fontSize = originSize;
                Destroy(this);
                enabled = false;
            }
            else
            {
                text.fontSize =Mathf.RoundToInt(
                    Mathf.Lerp(originSize * maxScale, originSize, (timer - timePingpongOnce) / timePingpongOnce));
            }
        }
        else
        {
            text.fontSize =Mathf.RoundToInt(
                Mathf.Lerp(originSize, originSize * maxScale, (timer / timePingpongOnce)));
        }
    }
}
