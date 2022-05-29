using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField]
    public SelectionStuffies SelectionPrefab;
    [SerializeField]
    private Agent Agent1;
    [SerializeField]
    private Agent Agent2;

    public static AgentManager Get() { return instance; }    
    private static AgentManager instance = null;

    public Agent GetCurrentAgent() => currentAgent;
    public Agent GetFirstAgent() => Agent1;
    public Agent GetSecondAgent() => Agent2;

    //swaps turns between agents
    public void SwapTurns()
    {
        //Debug.Log("swapping turns");
        if (currentAgent == Agent1)
        {            
            currentAgent = Agent2;            
        }
        else if (currentAgent == Agent2)
        {
            currentAgent = Agent1;
        }

        //Debug.Log(currentAgent);
        currentAgent.CurrentMoves = 2;
    }

    public bool IsGameOver()
    {
        if (Agent1.GetUnit() == null || Agent2.GetUnit() == null)
            return true;
        return false;
    }

    public Agent GetWinner()
    {
        if (Agent1.GetUnit() != null)
            return Agent1;
        if (Agent2.GetUnit() != null)
            return Agent2;
        return null;
    }

    public Agent currentAgent = null;

    //singelton
    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Should only be one AgentManager");
            
        }
        else
        {
            currentAgent = Agent1;
            instance = this;
        }        
    }

}
