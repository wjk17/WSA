using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Esa.Physic;
[ExecuteInEditMode]
public class TestUICol2d : MonoBehaviour
{
    public CircleCollider2D circle;
    public BoxCollider2D box;
    public bool isCollision;
    public float circleRadius;
    public Vector2 boxSize;
    // Use this for initialization
    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(box!=null)
        {
            circleRadius = circle.radius;
            boxSize = box.size;
            isCollision = circle.IsCollision(box);
        }
    }
}
