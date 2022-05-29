using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : GameItem
{
    [SerializeField]
    public int DEF_Increase = 1;
    [SerializeField]
    public float SpawnProbablity = 25;
    
    public override bool AddBuff(Unit unit)
    {
        unit.DEF += DEF_Increase;
        return true;
    }

    public override bool RemoveBuff(Unit unit)
    {
        unit.DEF -= DEF_Increase;
        return true;
    }
}