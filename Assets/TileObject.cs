using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : ObjectMaster
{
    
    [SerializeField]
    public ItemSub Item;
    [SerializeField]
    public TempBuffSub Buff;


    GameObject tile_object;



    public TileObject(int xpos, int ypos, GameObject prefab_tile, string name)
    {
        Xpos = xpos;
        Ypos = ypos;
        tile_object = prefab_tile;
        Type = "tile";
        Name = name;
    }
}
