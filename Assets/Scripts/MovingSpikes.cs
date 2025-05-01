using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 currentPosition;
    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentPosition = transform.position;
    }

    void Update()
    {
        float targetZ = Mathf.Clamp(playerTransform.position.z, 19f, 24f);

        currentPosition.z = targetZ;
        transform.position = currentPosition;
    }
}
