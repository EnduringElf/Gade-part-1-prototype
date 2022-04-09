using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject BoardPlacementPrefab;

    [SerializeField]
    private int HorizontalCount = 3;

    [SerializeField]
    private int VerticalCount = 3;

    private const int BoardPlacementSize = 10;

    void Start()
    {
        GameObject board = new GameObject();
        board.transform.name = "Board";        
                
        for (int i = 0; i < HorizontalCount; i++)
        { 
            for (int j = 0; j < VerticalCount; j++)
            {
                GameObject placement = Instantiate(BoardPlacementPrefab);
                placement.transform.position = new Vector3(i, 0, j) * BoardPlacementSize - 
                    new Vector3(BoardPlacementSize * (HorizontalCount / 2), 0, BoardPlacementSize * (VerticalCount / 2));

                placement.name = "Board placement " + $"{i}, {j}";
                placement.transform.SetParent(board.transform);
            }
        }
    }

    
    void Update()
    {
        
    }
}
