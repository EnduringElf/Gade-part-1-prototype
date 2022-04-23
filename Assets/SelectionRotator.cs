using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRotator : MonoBehaviour
{
    [SerializeField]
    public float RotationSpeed = 2.9283f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, RotationSpeed);
    }
}
