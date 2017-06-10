using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Crosshairs : MonoBehaviour {

    public float rotateVelocity = 40f;
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    Color originalDotColor;

    void Start()
    {
        Assert.IsNotNull(dot, "Crosshairs::Dot in crosshairs not assigned!");
        Cursor.visible = false;
        originalDotColor = dot.color;
    }

    void Update () {
        this.transform.Rotate(Vector3.forward * rotateVelocity * Time.deltaTime);
	}

    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighlightColor;
        } else
        {
            dot.color = originalDotColor;
        }
    }
}
