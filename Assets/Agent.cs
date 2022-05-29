using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    protected Unit unit;

    public int CurrentMoves = 2;

    public BoardPlacement Placement;        
    public abstract void Action(Map map);   

    // @NOTE: Look at this fancy affff syntax sugar.
    public void RegisterUnit(Unit unit) => this.unit = unit;
    public void RemoveUnit() => unit = null;

    public Unit GetUnit() => unit;

    public Unit selectedUnit = null;
    protected SelectionStuffies currentlySelected = null;

    // @NOTE: Swap turns if need be, else do nothing
    public void TurnLogic()
    {
        if (CurrentMoves <= 0)
        {
            if (currentlySelected)
            {
                Destroy(currentlySelected.gameObject);
            }

            selectedUnit = null;
            AgentManager.Get().SwapTurns();
        }
    }

    public static void OffsetUnits(Unit unit1, Unit unit2)
    {
        unit1.transform.position -= new Vector3(1f, 0.0f, 1f) * 2;
        unit2.transform.position += new Vector3(1f, 0.0f, 1f) * 2;
    }
}
