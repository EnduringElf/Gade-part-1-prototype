using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public static AgentManager Get() { return instance; }    
    private static AgentManager instance = null;

    public Agent GetCurrentAgent()
    {
        return currentAgent;
    }

    public Agent GetFirstAgent()
    { 
        return agent1;
    }    
    public Agent GetSecondAgent()
    {
        return agent2;
    }

    [SerializeField]
    private Agent agent1;
    [SerializeField]
    private Agent agent2;

    private Agent currentAgent = null;

    void Awake()
    {
        // @TEMP:
        agent1 = new PlayerAgent();
        agent2 = new PlayerAgent();

        if (instance)
        {
            Debug.LogError("Should only be one AgentManager");
            
        }
        else
        {
            currentAgent = agent1;
            instance = this;
        }        
    }
        
    void Update()
    {
        
    }
}
