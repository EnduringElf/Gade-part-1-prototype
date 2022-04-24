using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{

    private Unit currentunit = null;
    public BoardPlacement placement = null;

    public static ButtonLogic Get() { return instance; }
    private static ButtonLogic instance = null;

    PlayerAgent CPlayerAgent;

    //private bool HasItem = false;

    public Button Pickupbutton;
    public Button AttackButton;
    public Button LeaveItem;
    public Button Heal;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Should only be one button logic");
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        disableButton(Pickupbutton);
        disableButton(AttackButton);
        disableButton(LeaveItem);
        disableButton(Heal);
    }

    private void disableButton(Button button)
    {
        button.interactable = false;
    }
    public void enableButton(Button button)
    {
        button.interactable = true;
    }

    

    public void Set(PlayerAgent playerAgent)
    {
        CPlayerAgent = playerAgent;
    }

    

    public void PickUpButton()
    {
        CPlayerAgent.pickup(CPlayerAgent.Placement);
        Pickupbutton.interactable = false;

    }



}
