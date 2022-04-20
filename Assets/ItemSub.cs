using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemSub : ObjectMaster
{
    int value;
    public int ATK;
    public int DEF;
    public int HP;

    public ItemSub(int Value, string name)
    {
        value = Value;
        Name = name;
        Type = "Item";
        Addstat(value,Name) ;
    }

    private void Addstat(int value, string Name)
    {
        switch (Name) 
        {
            case "Sword":
                ATK = value;
                break;
            case "Dagger":
                ATK = value / 2;
                break;
            case "Shield":
                DEF = value;
                break;
            case "HPpot":
                HP = value;
                break;

        }

    }
}
