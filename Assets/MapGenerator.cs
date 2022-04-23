using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject BoardPlacementPrefab;

    public GameObject ClientPrefab;

    private Game_manager Manager;

    private TileObject[,] BoardRep;

    [SerializeField]
    private int HorizontalCount = 3;

    [SerializeField]
    private int VerticalCount = 3;

    private const int BoardPlacementSize = 10;

    void Start()
    {
        Manager = GameObject.Find("Admin").GetComponent<Game_manager>();

        GameObject board = new GameObject();
        board.transform.name = "Board";
        BoardRep = new TileObject[HorizontalCount,VerticalCount];
                
        for (int i = 0; i < HorizontalCount; i++)
        { 
            for (int j = 0; j < VerticalCount; j++)
            {
                GameObject placement = Instantiate(BoardPlacementPrefab);
                placement.transform.position = new Vector3(i, 0, j) * BoardPlacementSize - 
                    new Vector3(BoardPlacementSize * (HorizontalCount / 2), 0, BoardPlacementSize * (VerticalCount / 2));

                BoardRep[i, j] = new TileObject(i, j, placement, $"{i}, {j}");

                placement.name = "Board placement " + $"{i}, {j}";
                placement.transform.SetParent(board.transform);
            }
        }

        Manager.tiles = BoardRep;

        for(int i = 0; i < 2; i++)
        {
            GameObject client = Instantiate(ClientPrefab);
            client.transform.position = new Vector3(0, 1, 0);
            client.GetComponent<CleintSub>().name = "client " + $"{i +1}";
            
            Manager.clients[i] = client;

        }

    }

    
    void Update()
    {
        
    }
}
