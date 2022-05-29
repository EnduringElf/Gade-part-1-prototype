using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameItem : MonoBehaviour
{
    [SerializeField]
    public string Name;
    // @NOTE: Returns if the item is consumed on use
    [SerializeField]
    public float RotationSpeed = 3;

    // @NOTE: Virtual function that the specfic game items will implement when adding a buff
    public abstract bool AddBuff(Unit unit);

    // @NOTE: Virtual function that the specfic game items will implement when removing a buff
    public abstract bool RemoveBuff(Unit unit);

    public void Update()
    {        
        // @NOTE: Spinnning!!
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), RotationSpeed);
    }

}


