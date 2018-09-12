using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class RectInspector : MonoBehaviour
{
    [Header("Anchors")]
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public Vector2 offsetMin;
    public Vector2 offsetMax;
    public Vector2 pivot;

    [Header("Position")]
    public Vector2 anchoredPosition;
    public Vector2 sizeDelta;

    private RectTransform rect;
    void Start()
    {
        rect = transform as RectTransform;
    }
    bool modify;
    void Update()
    {
        if (modify)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
        }
        else
        {
            anchorMin = rect.anchorMin;
            anchorMax = rect.anchorMax;
            offsetMin = rect.offsetMin;
            offsetMax = rect.offsetMax;
            pivot = rect.pivot;
            anchoredPosition = rect.anchoredPosition;
            sizeDelta = rect.sizeDelta;
        }
    }
}
