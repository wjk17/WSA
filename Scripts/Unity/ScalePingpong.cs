using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePingpong : MonoBehaviour
{
    public float timePingpongOnce = 0.55f;
    public bool doWhenEnable = true;
    public float maxScale = 1.5f;
    float originScale;
    bool on;
    float timer;
    void OnEnable()
    {
        originScale = transform.localScale.x;
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
                transform.localScale = originScale * Vector3.one;
                Destroy(this);
                enabled = false;
            }
            else
            {
                transform.localScale = Vector3.one *
                    Mathf.Lerp(originScale * maxScale, originScale, (timer - timePingpongOnce) / timePingpongOnce);
            }
        }
        else
        {
            transform.localScale = Vector3.one *
                Mathf.Lerp(originScale, originScale * maxScale, (timer / timePingpongOnce));
        }
    }
}
