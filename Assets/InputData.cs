using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputData : MonoBehaviour
{
    public float Player1X;
    public float Player1Y;
    public int Player1HP;
    public int Player1ATK;
    public int Player1DEF;

    public float Player2X;
    public float Player2Y;
    public int Player2HP;
    public int Player2ATK;
    public int Player2DEF;

    public List<float> TileValues;
    public List<Tile> tiles = new List<Tile>();

    public GameObject Player1;
    public GameObject Player2;

    public List<GameObject> BoardTiles;

    public List<double> GetInputData()
    {
        List<double> Nurons = new List<double>();
        Nurons.Add(Player1HP);
        Nurons.Add(Player1ATK);
        Nurons.Add(Player1DEF);
        Nurons.Add(Player2ATK);
        Nurons.Add(Player2DEF);
        Nurons.Add(Player2HP);

        foreach (float t in TileValues)
        {
            Nurons.Add(t);
        }

        return Nurons;
    }


    //tiles are determined by distance to player and if
    //they have an item used in hidden layer to determine at what point which tile is worth going to



    // Start is called before the first frame update
    void Start()
    {
        
        
       
    }

    public void SetGameObjectsInstance()
    {
        if (GameObject.FindGameObjectWithTag("Player1") && GameObject.FindGameObjectWithTag("Player2"))
        {
            if (GameObject.FindGameObjectWithTag("Player1").tag == "Player1")
            {
                Player1 = GameObject.FindGameObjectWithTag("Player1");
            }
            if (GameObject.FindGameObjectWithTag("Player2").tag == "Player2")
            {
                Player2 = GameObject.FindGameObjectWithTag("Player2");
            }

            foreach(GameObject  t in GameObject.FindGameObjectsWithTag("BoardTile"))
            {
                BoardTiles.Add(t);

            }
        }

        SetInputData();

    }

    private void SetInputData()
    {
        if (Player1)
        {
            //Player1X = Player1.transform.position.x;
            //Player1Y = Player1.transform.position.y;
            Player1HP = Player1.GetComponent<Unit>().HP;
            Player1ATK = Player1.GetComponent<Unit>().ATK;
            Player1DEF = Player1.GetComponent<Unit>().DEF;
        }

        if (Player2)
        {
            //Player2X = Player2.transform.position.x;
            //Player2Y = Player2.transform.position.y;
            Player2HP = Player2.GetComponent<Unit>().HP;
            Player2ATK = Player2.GetComponent<Unit>().ATK;
            Player2DEF = Player2.GetComponent<Unit>().DEF;
        }
        foreach(GameObject t in BoardTiles)
        {
            int item = 0;
            int player = 0;
            if (t.GetComponent<BoardPlacement>().item)
            {
                if(t.GetComponent<BoardPlacement>().item.Name == "Sword")
                {
                    item = 3;
                }
                if (t.GetComponent<BoardPlacement>().item.Name == "HP_pot")
                {
                    item = 2;
                }
                if (t.GetComponent<BoardPlacement>().item.Name == "Shield")
                {
                    item = 1;
                }

            }
            if (t.GetComponent<BoardPlacement>().GetUnit())
            {
                player = 1;
            }
            if (t.GetComponent<BoardPlacement>())
            {
                Tile temp = new Tile(t.GetComponent<BoardPlacement>().i, t.GetComponent<BoardPlacement>().j, item, player);
                tiles.Add(temp);
                //Debug.Log("added new tile");
            }
            
        }
        foreach (Tile i in tiles)
        {
            TileValues.Add(i.GetValue());
            //Debug.Log(i.GetValue());
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

public class Tile
{
    int x;
    int y;
    int Item;
    int Player;

    public Tile(int x, int y, int Item, int player)
    {
        //this.x = x;
        //this.y = y;
        this.Item = Item;
        this.Player = player;
    }

    public float GetValue()
    {
        return Item + Player;
    }

    
}
