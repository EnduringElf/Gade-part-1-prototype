using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPlacement : MonoBehaviour
{
    private CleintSub unit;
    public List<BoardPlacement> neighbours;

    public CleintSub GetCurrentUnit()
    {
        return unit;
    }

    public void SetUnit(CleintSub unit, bool snap = false)
    {
        this.unit = unit;
        if (snap)
        {
            unit.transform.position = transform.position;
            unit.CurrentPlacement = this;
        }
    }

    void Start()
    {
    }
    
    void Update()
    {
    }

}
