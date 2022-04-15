using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleintSub : ObjectMaster
{
    [SerializeField]
    int HP = 10;
    [SerializeField]
    int ATK = 2;
    [SerializeField]
    int DEF = 1;
    [SerializeField]
    ItemSub Weapon;
    [SerializeField]
    TempBuffSub Buff;
    [SerializeField]
    int Buff_Duration = 3;

    //passing through weapons from board to charchters
    public void Addweapon(ItemSub item)
    {
        Weapon = item;
    }

    public void AddBuff(TempBuffSub TBuff)
    {
        Buff = TBuff;
    }
    //finding adjustment change to stats
    private void FindAdjustment(ItemSub item)
    {

    }
    

}
