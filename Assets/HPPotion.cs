using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HPPotion : GameItem
{
    [SerializeField]
    public int HP_Restored = 1;
    [SerializeField]
    public float SpawnProbablity = 25;

    public override bool AddBuff(Unit unit)
    {
        unit.HP += HP_Restored;
        return true;
    }

    public override bool RemoveBuff(Unit unit)
    {
        return false;
    }
}
