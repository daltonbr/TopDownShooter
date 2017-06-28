using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DebugSphere : MonoBehaviour
{
    public TextMesh score;  
    public MeshRenderer meshRenderer;
    public Color lerpedColor = Color.white;

    void Awake()
    {
        Assert.IsNotNull(meshRenderer, "[DebugSphere] meshRenderer is null!");
    }

    public void UpdateColor(Color color)
    {

    }

    public void SetScore(float score)
    {
        this.score.text = score.ToString("0");
    }

    public void SetScore(string score)
    {
        this.score.text = score;
    }

    public void UpdateTransparency(float alphaValue)
    {

    }


    void Update()
    {
        //lerpedColor = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time, 1));
        //material.color = lerpedColor;
    }

}