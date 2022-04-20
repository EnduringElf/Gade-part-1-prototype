using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBuffSub : ObjectMaster
{
    int value;
    public int ATK;
    public int DEF;
    public int HP;
    public int duration;

    public TempBuffSub(int Value,string name)
    {
        this.value = Value;
        Name = name;
        Addstat();
    }

    private void Addstat()
    {
        switch (Name) 
        {
            case "ATKbuff":
                ATK = value;
                duration = 2;
                break;
            case "DEFbuff":
                DEF = value;
                duration = 4;
                break;
            case "HPbuff":
                HP = value;
                duration = 3;
                break;

        }

    }
}
