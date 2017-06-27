using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DebugSpheres : MonoBehaviour
{
    [Range(0f, 1f)]
    public float transparency = 0.3f;
    public GameObject[] spheres;
    public float radius = 0.3f;
    public GameObject prefab;
    public MeshRenderer meshRenderer;
    //public color lowestColor;
    //public color highestColor;
    //public color choosenSphereColor;
    private GameObject holder;
    private AIManager aiManager;

    private int lateralGrid;
    private int maxNumberOfSpheres;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        aiManager = GetComponent<AIManager>();
        lateralGrid = (int)(aiManager.samplingRange / aiManager.samplingDensity) + 1;
        maxNumberOfSpheres = lateralGrid * lateralGrid;

        holder = new GameObject();
        holder.name = "DebugSpheresHolder";

        InstantiateNewSpheres(maxNumberOfSpheres);

        Assert.IsNotNull(aiManager, "[DebugSpheres] aiManager is null");
        Assert.IsNotNull(meshRenderer, "[DebugSpheres] meshRenderer is null");
    }

    private void InstantiateNewSpheres(int numberOfSpheres)
    {
        for (int i = 0; i < numberOfSpheres; i++)
        {
            GameObject sphere = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity, holder.transform);
            sphere.name = "DebugSphere_" + numberOfSpheres.ToString("D3");
            spheres[i] = sphere;
        }
    }

    public void UpdateSpheres(List<Vector3> positions)
    {
        Debug.Log("Update Spheres");
        if (positions.Count > spheres.Length)
        {
            Debug.LogWarning("There are more sampledPositions than Instantiated Spheres");
            return;
        }
        else
        {
            for (int i = 0; i < positions.Count; i++)
            {
                spheres[i].transform.position = positions[i];
            }
        }

    }

    //public void DeactiveAllSpheres()
    //{

    //}
    
   
}
