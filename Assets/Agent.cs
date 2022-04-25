using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    protected List<Unit> units = new List<Unit>();

    public int CurrentMoves = 2;

    public BoardPlacement Placement;

    

    public abstract void Action(BoardPlacement placement);
    public void RegisterUnit(Unit unit)
    {
        units.Add(unit);
    }
}
