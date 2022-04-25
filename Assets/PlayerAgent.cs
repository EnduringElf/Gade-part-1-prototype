using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAgent : Agent
{
    [SerializeField]
    public SelectionStuffies SelectionStuffiesPrefab = null;

    private Unit currentUnit = null;

    public Unit unit1;
    public Unit unit2;

    private SelectionStuffies currentlySelected = null;

    public BoardPlacement origin;

    public bool transformoffset = false;

    bool swap = true;

    private void Start()
    {
        //findplayercurrentpos();

    }

    public void findplayercurrentpos()
    {
        //unit1 = GameObject.Find("Player1Unit(Clone)").GetComponent<Unit>();
        //unit2 = GameObject.Find("Player2Unit(Clone)").GetComponent<Unit>();

        //Debug.Log("unit 1 current placement" + unit1.CurrentPlacement);
        //Debug.Log("unit 2 current placement" + unit2.CurrentPlacement);

    }

    private void Update()
    {
        if(swap == true)
        {
            if (CurrentMoves <= 0)
            {
                ButtonLogic.Get().AttackButton.interactable = false;
                if (currentlySelected)
                {
                    Destroy(currentlySelected.gameObject);
                }
                //swaps turn here
                currentUnit = null;

                Debug.Log("this name is " + this.name);
                AgentManager.Get().SwapTurns();
                swap = false;
            }
            else
            {
                if (GameObject.Find("Map Spawner").GetComponent<Map>().otherunit && GameObject.Find("Map Spawner").GetComponent<Map>().currentselecretedunit)
                {
                    ButtonLogic.Get().AttackButton.interactable = true;
                    ButtonLogic.Get().Set(this);
                }
                else
                {
                    ButtonLogic.Get().AttackButton.interactable = false;
                }
            }
        }
        
        

        //if(unit1.CurrentPlacement == unit2.CurrentPlacement)
        //{
        //    ButtonLogic.Get().AttackButton.interactable = true;
        //    ButtonLogic.Get().Set(this);


        //}
        //if (transformoffset == true)
        //{
        //    if (unit1.CurrentPlacement == unit2.CurrentPlacement)
        //    {
        //        transformplayeroffet(unit1);

        //    }
        //    else
        //    {
        //        transformplayeroffet(unit2);
        //    }
            
        //}
               

    }

    private void transformplayeroffet(Unit unit)
    {
        
        if (transformoffset == true)
        {
            if (unit == unit1)
            {
                unit1.transform.position -= new Vector3(1f, 0.0f, 1f) * 2;
                
                
            }else if (unit == unit2)
            {
                unit2.transform.position += new Vector3(1f, 0.0f, 1f) * 2;


            }
            else
            if (unit1.CurrentPlacement == unit2.CurrentPlacement)
            {
                unit1.transform.position -= new Vector3(1f, 0.0f, 1f) * 2;
                unit2.transform.position += new Vector3(1f, 0.0f, 1f) * 2;
            }

            transformoffset = false;
        }
    }

    public override void Action(BoardPlacement placement)
    {

        Placement = placement;
        //works on more pieces on board if there are more peice to control like rpg chess
        //no firnedly logic yet
        Unit unit = placement.GetFirstUnit();
        //checks if current unit != null
        if (currentUnit != null)
        {
            if (currentUnit == unit)
            {
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
                        //check for other player on/going onto same square
                        if (currentUnit.MoveTo(placement, ref CurrentMoves))
                        {
                            
                            //logic for attacking on same square
                            if (CurrentMoves <= 0)
                            {
                                if (currentlySelected)
                                {
                                    Destroy(currentlySelected.gameObject);
                                }
                                //swaps turn here
                                currentUnit = null;
                                AgentManager.Get().SwapTurns();
                            }

                            //ButtonLogic.Get().AttackButton.interactable = true;
                            //ButtonLogic.Get().Set(this);
                            findplayercurrentpos();
                            transformoffset = true;

                            Unit unit1 = placement.GetFirstUnit();
                            //Debug.Log("First " + unit1);
                            Unit unit2 = placement.GetSecondUnit();
                            //Debug.Log("Second" + unit2);
                            ///
                            unit1.transform.position -= new Vector3(1f, 0.0f, 1f) * 2;
                            unit2.transform.position += new Vector3(1f, 0.0f, 1f) * 2;



                        }
                    }                    
                }
                else
                {


                    // @NOTE: Placement is empty therefore move and can still move
                    if (currentUnit.MoveTo(placement, ref CurrentMoves))
                    {
                        //if space has an item add that to visual representation spot
                        if (currentUnit.Item == null && placement.item != null)
                        {
                            ButtonLogic.Get().Pickupbutton.interactable = true;
                            ButtonLogic.Get().Set(this);
                            
                        }
                        //destroys the selction object that cricles the player
                        if (CurrentMoves <= 0)
                        {
                            if (currentlySelected)
                            {
                                Destroy(currentlySelected.gameObject);
                            }
                            //then swaps turns through agent manager
                            ButtonLogic.Get().Pickupbutton.interactable = false;
                            currentUnit = null;
                            AgentManager.Get().SwapTurns();
                        }
                    }
                    else
                    {
                        //debug log if item or space is invalid
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
    public void pickup(BoardPlacement placement)
    {
        //Debug.Log("picked up " + $"{placement.item.name}");
        //sets the unit item to board itme
        currentUnit.Item = placement.item;
        //will auto load item and set the unit to be unable to pick up items
        currentUnit.Item.enabled = false;
        //sets the parent and local postion so player will drag that itme around
        placement.item.transform.SetParent(currentUnit.WeaponVisualSpot, true);
        placement.item.transform.localPosition = new Vector3(0, 0, 0);
        //do adjustement to stats

    }

    public void Attack(BoardPlacement placement)
    {
        if (CurrentMoves <= 0)
        {
            ButtonLogic.Get().AttackButton.interactable = false;
            if (currentlySelected)
            {
                Destroy(currentlySelected.gameObject);
            }
            //swaps turn here
            currentUnit = null;
            AgentManager.Get().SwapTurns();
        }

        //Debug.Log("this was from" + this.name);
        //Debug.Log("this was from placement" + Placement);

        GameObject.Find("Map Spawner").GetComponent<Map>().otherunit.HP -= GameObject.Find("Map Spawner").GetComponent<Map>().currentselecretedunit.ATK;
        
        CurrentMoves--;
        Debug.Log("current moves remaing" + CurrentMoves);

        //Debug.Log("the player" + GameObject.Find("Map Spawner").GetComponent<Map>().currentselecretedunit.name + "attacked" + GameObject.Find("Map Spawner").GetComponent<Map>().otherunit.name);

        //if(Placement.GetFirstUnit() == this)
        //{
        //    GameObject.Find("Player2Unit(Clone)").GetComponent<Unit>().HP -= GameObject.Find("Player1Unit(Clone)").GetComponent<Unit>().ATK;

        //}
        //else
        //{
        //    GameObject.Find("Player1Unit(Clone)").GetComponent<Unit>().HP -= GameObject.Find("Player2Unit(Clone)").GetComponent<Unit>().ATK;
        //}



        //if()
        //{
        //    Debug.Log("unit 1 name: " + currentUnit);
        //}
        //else
        //{
        //    Debug.Log("unit 2 name: " + currentUnit);
        //}


        //Debug.Log("unit 1 name: " + unit1.name);
        //Debug.Log("unit 2 name: " + unit2.name);



    }

}
