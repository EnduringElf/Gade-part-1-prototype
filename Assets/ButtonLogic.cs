using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public static ButtonLogic Get() { return instance; }
    private static ButtonLogic instance = null;

    public TMPro.TextMeshProUGUI Endtext;
    public Button Pickupbutton;
    public Button AttackButton;
    public Button DropItemButton;
    public Button HealButton;

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

    private void DisableButtons()
    {
        Pickupbutton.interactable = false;
        AttackButton.interactable = false;
        DropItemButton.interactable = false;
        HealButton.interactable = false;
    }

    private void Start()
    {       
        DisableButtons();
    }

    private void Update()
    {
        // @NOTE: Assume all buttons are going to be disabled then reactivate when need be
        DisableButtons();
        AgentManager agentManager = AgentManager.Get();

        Agent agent = agentManager.GetCurrentAgent();
        if (agent is PlayerAgent playerAgent)
        {
            if (playerAgent.selectedUnit != null)
            {
                // @NOTE: If player have something selected
                Unit unit = playerAgent.selectedUnit;

                if (unit.CurrentPlacement.HasItem())
                {
                    // @NOTE: If the player has item check if the new item is a health potion or not
                    GameItem item = unit.CurrentPlacement.GetItem();
                    if (item is HPPotion)
                    {
                        HealButton.interactable = true;
                    }
                    else
                    {
                        Pickupbutton.interactable = true;
                    }
                }

                if (unit.HasItem() && !unit.CurrentPlacement.HasItem())
                {
                    // @NOTE: Only drop the item if there is not other item on the floor, else just pick it up to swap them
                    DropItemButton.interactable = true;
                }

                if (unit.CurrentPlacement.GetUnitCount() >= 2)
                {
                    // @NOTE: Can only attack if there are more that 2 units 
                    AttackButton.interactable = true;
                }
            }
        }

        if (agentManager.IsGameOver())
        {
            // @NOTE: If game is over display the winner text
            Agent winner = agentManager.GetWinner();
            Endtext.text = winner + " is the winner !!";
            DisableButtons();
        }
    }

    public void OnPickUp()
    {
        Agent agent = AgentManager.Get().GetCurrentAgent();
        if (agent is PlayerAgent playerAgent)
        {
            playerAgent.selectedUnit.EquipItem();
        }
    }

    public void OnHeal()
    {
        Agent agent = AgentManager.Get().GetCurrentAgent();
        if (agent is PlayerAgent playerAgent)
        {
            playerAgent.selectedUnit.Heal();
        }
    }

    public void OnDrop()
    {
        Agent agent = AgentManager.Get().GetCurrentAgent();
        if (agent is PlayerAgent playerAgent)
        {
            playerAgent.selectedUnit.DropItem();
        }
    }

    public void OnAttack()
    {
        Agent agent = AgentManager.Get().GetCurrentAgent();
        if (agent is PlayerAgent playerAgent)
        {
            playerAgent.selectedUnit.Attack();
        }
    }
}
