using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public GameObject[] clients;

    public TileObject[,] tiles;

    public int currentplayer = 1;

    // Start is called before the first frame update
    void Start()
    {
        clients = new GameObject[2];
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveplayer(float x, float y)
    {
        clients[currentplayer].GetComponent<CleintSub>().Move(x, y);
    }


}
