using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionStuffies : MonoBehaviour
{
    [SerializeField]
    public float RotationSpeed = 1.3983f;

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
             transform.GetChild(i).RotateAround(transform.position, new Vector3(0,1,0), RotationSpeed);            
        }
    }
}
