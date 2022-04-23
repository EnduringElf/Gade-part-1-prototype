using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Potion : GameItem
{
    [SerializeField]
    private int HP_Restored = 1;
    [SerializeField]
    public float SpawnProbablity = 25;

    public override bool AddBuff(Unit unit)
    {
        unit.HP += HP_Restored;
        return true;
    }
}
