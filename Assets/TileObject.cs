using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    [SerializeField]
    private int Xpos;
    [SerializeField]
    private int Ypos;

    public TileObject(int xpos, int ypos)
    {
        Xpos = xpos;
        Ypos = ypos;

    }
}
