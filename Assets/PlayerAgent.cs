using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : Agent
{
    [SerializeField]
    public SelectionStuffies SelectionStuffiesPrefab = null;

    private Unit currentUnit = null;
    private SelectionStuffies currentlySelected = null;

    public override void Action(BoardPlacement placement)
    {
        Unit unit = placement.GetFirstUnit();

        if (currentUnit != null)
        {
            if (currentUnit == unit)
            {
                // @NOTE: Deseclt this, make pretty in game elf!!! :)
                Destroy(currentlySelected.gameObject);
                currentUnit = null;   
            }
            else
            {
                // @NOTE: More game logic
                if (unit != null)
                {
                    // @NOTE: There is a unit.
                    if (unit.OwningAgent == currentUnit.OwningAgent)
                    {
                        // @NOTE: Tis a freindly unit
                    }
                    else if (unit.OwningAgent != null)
                    {
                        if (currentUnit.MoveTo(placement, ref CurrentMoves))
                        {
                            if (CurrentMoves <= 0)
                            {
                                if (currentlySelected)
                                {
                                    Destroy(currentlySelected.gameObject);
                                }

                                currentUnit = null;
                                AgentManager.Get().SwapTurns();
                            }

                            Unit unit1 = placement.GetFirstUnit();
                            Debug.Log("First " + unit1);
                            Unit unit2 = placement.GetSecondUnit();
                            Debug.Log("Second" + unit2);

                            unit1.transform.position -= new Vector3(1f, 0.0f, 1f) * 2;
                            unit2.transform.position += new Vector3(1f, 0.0f, 1f) * 2;
                        }
                    }                    
                }
                else
                {
                    // @NOTE: Placement is empty therefore move
                    if (currentUnit.MoveTo(placement, ref CurrentMoves))
                    {
                        if (currentUnit.Item == null && placement.item != null)
                        {
                            currentUnit.Item = placement.item;
                            currentUnit.Item.enabled = false;
                            placement.item.transform.SetParent(currentUnit.WeaponVisualSpot, true);
                            placement.item.transform.localPosition = new Vector3(0, 0, 0);
                        }

                        if (CurrentMoves <= 0)
                        {
                            if (currentlySelected)
                            {
                                Destroy(currentlySelected.gameObject);
                            }

                            currentUnit = null;
                            AgentManager.Get().SwapTurns();
                        }
                    }
                    else
                    {
                        Debug.Log("s");
                    }
                }
            }
        }
        else
        {
     
            if (unit != null && unit.OwningAgent == this)
            {
                currentlySelected = Instantiate(SelectionStuffiesPrefab, unit.transform);
                currentUnit = unit;
            }
   

        }
    }
}
