using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleintSub : ObjectMaster
{
    [SerializeField]
    int maxHP = 10;
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
    int Buff_Duration = 0;
    [SerializeField]
    int tileLength = 1;

    [SerializeField]
    int offset;
    bool OffsetTrue = true;

    


    private void Update()
    {
        if(OffsetTrue == true)
        {
            switch (Name)
            {
                case "client 2":
                    offset = -2;
                    break;
                case "client 1":
                    offset = 2;
                    break;
            }

        }
       

        Name = this.gameObject.name;
        this.transform.position = new Vector3(((Xpos)) + offset, 0,((Ypos))+offset);
    }

    //passing through weapons from board to charchters
    public void Addweapon(ItemSub item)
    {
        Weapon = item;
        ItemAdj(Weapon);
    }

    public void AddBuff(TempBuffSub TBuff)
    {
        Buff = TBuff;
        buffadd(Buff);
    }

    private void buffadd(TempBuffSub t)
    {
        ATK += t.ATK;
        DEF += t.DEF;
        maxHP += t.HP;
        heal(t.HP);
        Buff_Duration = t.duration;
    }
    private void buffminus()
    {
        ATK -= Buff.ATK;
        DEF -= Buff.DEF;
        maxHP -= Buff.HP;
    }
    private void heal(int value)
    {
        for(int i = 0; i< value; i++)
        {
            if(HP !>= maxHP)
            {
                HP++;
            }
            else
            {

            }
        }
    }

    //finding adjustment change to stats
    private void ItemAdj(ItemSub item)
    {
        ATK += item.ATK;
        DEF += item.DEF;
        HP += item.HP;
    }

    private void attack(CleintSub enemny)
    {
        enemny.HP -= ATK;
    }

    public void Move(float x, float y)
    {
        Xpos = x;
        Ypos = y;
    }






}
