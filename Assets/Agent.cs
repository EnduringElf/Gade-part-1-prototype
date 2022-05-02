using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    protected List<Unit> units = new List<Unit>();

    public int CurrentMoves = 2;

    public BoardPlacement Placement;        
    public abstract void Action(BoardPlacement placement);

    // @NOTE: Look at this fancy affff syntax sugar.
    public void RegisterUnit(Unit unit) => units.Add(unit);
    public void RemoveUnit(Unit unit) => units.Remove(unit);

    public List<Unit> GetUnits() => units;

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

}
