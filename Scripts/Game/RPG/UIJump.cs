using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa;

public class UIJump : MonoBehaviour
{
    public float originY;
    RectTransform rt;
    public float t;
    public float jumpHeight = 30;
    public float speed = 0.6f;
    // Use this for initialization
    void Start()
    {
        t = 2;
        rt = (transform as RectTransform);
        originY = rt.anchoredPosition.y;

    }
    public void Jump()
    {
        t = 0;
        GetComponent<Text>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        float y;
        t += Time.deltaTime * speed;
        if (t < 1)
            y = Mathf.Lerp(originY, originY + jumpHeight, t);
        else y = Mathf.Lerp(originY + jumpHeight, originY, t - 1);

        if (t >= 2.5f) GetComponent<Text>().enabled = false;

        rt.anchoredPosition = rt.anchoredPosition.SetY(y);

    }
}
