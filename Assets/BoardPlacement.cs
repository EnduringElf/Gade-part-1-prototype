using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlacement : MonoBehaviour
{
    public Unit unit1;
    public Unit unit2;

    public GameItem item;

    public List<BoardPlacement> neighbours;

    public Unit GetFirstUnit()
    {
        return unit1;
    }

    public Unit GetSecondUnit()
    {
        return unit2;
    }

    public Unit[] GetUnits()
    {
        return new Unit[] { unit1, unit2 };
    }

    public int GetUnitCount()
    {
        int count = 0;
        if (unit1) count++;
        if (unit2) count++;

        return count;
    }

    public void SetUnit(Unit unit, bool snap = false)
    {
        if (unit == null)
        {
            if (unit2)
            {
                unit1 = unit2;
            }
            else if (unit1)
            {
                unit1 = null;
            }

            unit2 = null;
            return;
        }

        if (snap)
        {
            unit.transform.position = transform.position;
            unit.CurrentPlacement = this;
        }

        if (unit1 == null)
        {
            unit1 = unit;
            return;
        }

        if (unit2 == null)
        {
            unit2 = unit;
            return;
        }
    }

    void Start()
    {
    }
    
    void Update()
    {
    }

}
