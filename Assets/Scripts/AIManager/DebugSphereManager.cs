using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DebugSphereManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float transparency = 0.3f;
    public DebugSphere[] spheres;
    public float radius = 0.3f;
    public GameObject debugSpherePrefab;
        
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
            spheres[i] = sphere.GetComponent<DebugSphere>();
        }
    }

    public void UpdateSpheres(List<Vector3> positions, float[] scores)
    {
        Debug.Log("Update Spheres");
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
            /* Reallocate spheres */
            for (int i = 0; i < positions.Count; i++)
            {
                spheres[i].transform.position = positions[i];
                spheres[i].SetScore(scores[i]);
                spheres[i].gameObject.SetActive(true);
            }

            /* Deactivate unused spheres */
            for (int i = positions.Count; i < spheres.Length; i++)
            {
                spheres[i].gameObject.SetActive(false);
            }
        }

    }
}
