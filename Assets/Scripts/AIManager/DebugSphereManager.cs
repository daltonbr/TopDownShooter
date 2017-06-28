using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DebugSphereManager : MonoBehaviour
{
    const float MINIMUM_SCORE = 200f;
    const float MAXIMUM_SCORE = 500f;

    [Range(0f, 1f)]
    public float transparency = 0.3f;
    public DebugSphere[] spheres;
    public float radius = 0.5f;
    public GameObject debugSpherePrefab;
        
    public MeshRenderer meshRenderer;
    public Color lowestColor;
    public Color highestColor;
    public Color choosenSphereColor;
    private GameObject holder;
    private AIManager aiManager;

    private int lateralGrid;
    private int maxNumberOfSpheres;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        aiManager = GetComponent<AIManager>();
        this.radius = aiManager.radius;
        this.transparency = aiManager.transparency;
        this.debugSpherePrefab = aiManager.debugPrefab;
        lateralGrid = (int)(aiManager.samplingRange / aiManager.samplingDensity);
        maxNumberOfSpheres = lateralGrid * lateralGrid;
        Debug.Log("maxNumberOfSpheres: " + maxNumberOfSpheres);

        holder = new GameObject();
        holder.name = "DebugSpheresHolder";

        InstantiateNewSpheres(maxNumberOfSpheres);

        Assert.IsNotNull(aiManager, "[DebugSphereManager] aiManager is null");
        Assert.IsNotNull(meshRenderer, "[DebugSphereManager] meshRenderer is null");
        Assert.IsNotNull(this.debugSpherePrefab, "[DebugSphereManager] debugSpherePrefab is null");
    }

    private void InstantiateNewSpheres(int numberOfSpheres)
    {
        spheres = new DebugSphere[maxNumberOfSpheres];
        for (int i = 0; i < numberOfSpheres; i++)
        {
            GameObject sphere = Instantiate(debugSpherePrefab, this.transform.position, Quaternion.identity, holder.transform);
            sphere.name = "DebugSphere_" + i.ToString("D3");
            sphere.SetActive(false);
            sphere.transform.localScale = new Vector3 (radius, radius, radius);
            spheres[i] = sphere.GetComponent<DebugSphere>();
        }
    }

    void ChangeMaterial(MeshRenderer platformRenderer)
    {
        Color color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        platformRenderer.material.color = color;
    }

    public void UpdateSpheres(List<Vector3> positions, float[] scores)
    {

        if (positions.Count > spheres.Length)
        {
            Debug.LogWarning("[DebugSphereManager] There are more sampledPositions than Instantiated Spheres");
            return;
        }

        if (scores.Length < positions.Count)
        {
            Debug.LogWarning("[DebugSphereManager] There aren't enough score values");
            return;
        }
        else
        {
            //float minScore, maxScore, highScore;

            /* Reallocate spheres */
            for (int i = 0; i < positions.Count; i++)
            {
                spheres[i].transform.position = positions[i];
                spheres[i].SetScore(scores[i]);
                spheres[i].gameObject.SetActive(true);

                float lerpedValue = Mathf.InverseLerp(MINIMUM_SCORE, MAXIMUM_SCORE, scores[i]);
                //Debug.Log(lerpedValue);
                Color lerpedColor = Color.Lerp(Color.white, Color.red, lerpedValue);
                spheres[i].meshRenderer.material.color = lerpedColor;
            }

            /* Deactivate unused spheres */
            for (int i = positions.Count; i < spheres.Length; i++)
            {
                spheres[i].gameObject.SetActive(false);
            }
        }

    }

    //TODO: 
    //public SetMinMaxScores()
    //{

    //}
}
