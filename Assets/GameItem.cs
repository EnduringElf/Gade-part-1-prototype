using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    // @NOTE: Returns if the item is consumed on use
    [SerializeField]
    public float RotationSpeed = 3;

    public abstract bool AddBuff(Unit unit);

    public void Update()
    {        
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), RotationSpeed);
    }

}


