using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDifficulity : MonoBehaviour
{

    public Unit Player1;

    public MinMaxAgent MinMaxAgent;

    public int HP;

    public int ATK;

    public int depth;

    

    public void setDifficulity()
    {
        Player1 = GameObject.Find("Player1Unit(Clone)").GetComponent<Unit>();
        Player1.HP = this.HP;
        Player1.MaxHP = this.HP;
        Player1.ATK = this.ATK;
        MinMaxAgent.SearchDepth = this.depth;

        
    }
    

    
}
