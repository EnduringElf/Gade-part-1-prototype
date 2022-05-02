using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlacement : MonoBehaviour
{
    private Unit unit1;
    private Unit unit2;

    public GameItem item;

    public List<BoardPlacement> neighbours;

    public Unit GetFirstUnit() => unit1;
    public Unit GetSecondUnit() => unit2;
    public Unit[] GetUnits() => new Unit[] { unit1, unit2 };
    public bool HasItem() => item != null;
    public GameItem GetItem() => item;

    public int GetUnitCount()
    {
        int count = 0;
        if (unit1) count++;
        if (unit2) count++;

        return count;
    }

    public Unit GetUnit()
    {
        if (unit1 != null)
            return unit1;
        if (unit2 != null)
            return unit2;

        return null;
    }

    public void RemoveUnit(Unit unit)
    {
        //Debug.Log("in: " +unit);
        //Debug.Log("u1: " +unit1);
        //Debug.Log("u2: " +unit2);
        if (unit == unit1)
        {
            unit1 = null;
        }

        if (unit == unit2)
        {
            unit2 = null;
        }
    }

    public void SetUnit(Unit unit, bool snap = false)
    {
        if (unit1 == null)
            unit1 = unit;
        else if (unit2 == null)
            unit2 = unit;

        if (snap)
        {
            unit.transform.position = transform.position;
            unit.CurrentPlacement = this;
        }
    }
}
