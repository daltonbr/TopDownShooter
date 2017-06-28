using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DebugSphere : MonoBehaviour
{
    public TextMesh score;
    public MeshFilter meshFilter;

    void Awake()
    {
        //meshFilter = this.GetComponent<MeshFilter>();
        Assert.IsNotNull(meshFilter, "[DebugSphere] Mesh Filter is null!");
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

}