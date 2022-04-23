using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public BoardPlacement BoardPlacementPrefab;
    public CleintSub UnitPrefab;
        
    [SerializeField]
    private int HorizontalCount = 3;

    [SerializeField]
    private int VerticalCount = 3;

    private const int BoardPlacementSize = 10;

    private BoardPlacement[,] placements;

    CleintSub unit;

    void Start()
    {
        GameObject board = new GameObject();
        placements = new BoardPlacement[HorizontalCount, VerticalCount];

        board.transform.name = "Board";        
                
        for (int i = 0; i < HorizontalCount; i++)
        { 
            for (int j = 0; j < VerticalCount; j++)
            {
                BoardPlacement placement = Instantiate(BoardPlacementPrefab);
                placement.transform.position = new Vector3(i, 0, j) * BoardPlacementSize - 
                    new Vector3(BoardPlacementSize * (HorizontalCount / 2), 0, BoardPlacementSize * (VerticalCount / 2));

                placement.name = "Board placement " + $"{i}, {j}";
                placement.transform.SetParent(board.transform);

                if (i == 0 && j == 0)
                {
                    unit = Instantiate(UnitPrefab);
                    unit.MovesRemaining = 2;
                    placement.SetUnit(unit, true);
                    unit.OwningAgent = AgentManager.Get().GetFirstAgent();
                }

                placements[i, j] = placement;
            }
        }

        for (int i = 0; i < HorizontalCount; i++)
        {
            for (int j = 0; j < VerticalCount; j++)
            {
                if (i == 0 && j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                }
                else if (i == HorizontalCount - 1 && j == VerticalCount - 1)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                }
                else if (i == 0 && j == VerticalCount - 1)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                }
                else if (i == HorizontalCount - 1 && j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                }
                else if (i == 0)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    placements[i, j].neighbours.Add(placements[i + 1, j + 1]);
                }
                else if (i == HorizontalCount - 1)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                    placements[i, j].neighbours.Add(placements[i - 1, j - 1]);
                }
                else if (j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i + 1, j + 1]);
                }
                else if (j == VerticalCount - 1)
                {
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i - 1, j - 1]);
                }
                else
                {
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                }
            }
        }
    }
                
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit info;
            if (Physics.Raycast(ray, out info, LayerMask.GetMask("Board")))
            {
                BoardPlacement placement = info.collider.gameObject.GetComponent<BoardPlacement>();
                if (placement)
                {
                    AgentManager.Get().GetCurrentAgent().Action(placement);                 
                }
            }

           
        }
     


    }
}
