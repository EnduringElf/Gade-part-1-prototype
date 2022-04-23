using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : Agent
{
    private CleintSub currentUnit = null;

    public void Action(BoardPlacement placement)
    {
        CleintSub unit = placement.GetCurrentUnit();

        if (currentUnit != null)
        {
            if (currentUnit == unit)
            {
                // @NOTE: Deseclt this, make pretty in game elf!!! :)
                Debug.Log("Deselected unit" + unit);
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
                    else
                    {
                        // @NOTE: Tis an enemny unit
                    }
                }
                else
                {
                    // @NOTE: Placement is empty therefore move
                    currentUnit.MoveTo(placement);
                }
            }
        }
        else
        {
            
            if (unit != null)
            {
                Debug.Log("Selected unit" + unit);
                currentUnit = unit;
            }            
        }
    }


}
