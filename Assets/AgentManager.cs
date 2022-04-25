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

    public Agent GetCurrentAgent()
    {
        return currentAgent;
    }


    public Agent GetFirstAgent()
    { 
        return Agent1;
    }    

    public Agent GetSecondAgent()
    {
        return Agent2;
    }
    //swaps turns between agents
    public void SwapTurns()
    {
        Debug.Log("swapping turns");
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
        
    void Update()
    {
        
    }
}
