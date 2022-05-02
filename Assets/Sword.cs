using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : GameItem
{
    [SerializeField]
    private int ATK_Increase = 1;
    [SerializeField]
    public float SpawnProbablity = 15;

    public override bool AddBuff(Unit unit)
    {
        unit.ATK += ATK_Increase;
        return true;
    }

    public override bool RemoveBuff(Unit unit)
    {
        unit.ATK -= ATK_Increase;
        return true;
    }
}
