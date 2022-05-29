using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAgent : Agent 
{
    [SerializeField]
    public SelectionStuffies SelectionStuffiesPrefab = null;


    bool swap = false;

    private void Start()
    {

    }



    public override void Action(Map map)
    {
        //recayst to hot on mouse down
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit info;
            //if ray hit the board 
            if (Physics.Raycast(ray, out info, LayerMask.GetMask("Board")))
            {
                //sets the placement to boardplacement info from ray                
                BoardPlacement placement = info.collider.gameObject.GetComponent<BoardPlacement>();

                if (placement)
                {
                    DoAction(placement);
                }
            }
        }
    }

    public void DoAction(BoardPlacement placement)
    {        
        Placement = placement;
        //works on more pieces on board if there are more peice to control like rpg chess
        //no firnedly logic yet

        Unit unit = null;
        if (placement.GetUnitCount() >= 2)
        {
            unit = placement.GetFirstUnit().OwningAgent == this ? placement.GetFirstUnit() : placement.GetSecondUnit();
        }

        if (placement.GetUnitCount() == 1)
        {
            unit = placement.GetUnit();
        }


        //checks if current unit != null
        if (selectedUnit != null)
        {
            if (selectedUnit == unit)
            {
                Destroy(currentlySelected.gameObject);
                selectedUnit = null;   
            }
            else
            {
                // @NOTE: More game logic
                if (unit != null)
                {
                    // @NOTE: There is a unit.
                    if (unit.OwningAgent == selectedUnit.OwningAgent)
                    {
                        // @NOTE: Tis a freindly unit
                    }
                    else if (unit.OwningAgent != null)
                    {
                        // @NOTE: Tis a enemy unit
                        if (selectedUnit.MoveTo(placement, ref CurrentMoves))
                        {
                            TurnLogic();

                            Unit unit1 = placement.GetFirstUnit();
                            Unit unit2 = placement.GetSecondUnit();

                            OffsetUnits(unit1, unit2);
                        }
                    }                    
                }
                else
                {
                    // @NOTE: Placement is empty.
                    if (selectedUnit.MoveTo(placement, ref CurrentMoves))
                    {
                        TurnLogic();
                    }
                    else
                    {
                        Debug.Log("Can't move there");
                    }
                }
            }
        }
        else
        {
            // @NOTE: Nothing selected
            if (unit != null && unit.OwningAgent == this)
            {
                currentlySelected = Instantiate(SelectionStuffiesPrefab, unit.transform);
                selectedUnit = unit;
            } 
        }
    }  
}
