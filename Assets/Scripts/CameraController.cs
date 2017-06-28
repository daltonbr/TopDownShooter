using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    
    private Vector3 offset;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player == null)
        {
            Debug.LogError("[CameraController] Can't find a Player in the scene!");
        }
        Assert.IsNotNull(player, "[CameraController] Player is null");
    }

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    /* LateUpdate is called after Update each frame */
    void LateUpdate()
    {
        if (player)
        {
            this.transform.position = player.transform.position + offset;
        }
    }
}