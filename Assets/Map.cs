using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    public BoardPlacement BoardPlacementPrefab;
    [SerializeField]
    public Unit Player1UnitPrefab;
    [SerializeField]
    public Unit Player2UnitPrefab;
    [SerializeField]
    public int MaxSwordSpawnCount = 2;
    [SerializeField]
    public Sword SwordPrefab;
    [SerializeField]
    public int MaxPotionSpawnCount = 4;
    [SerializeField]
    public Potion PotionPrefab;
    [SerializeField]
    public int MaxShieldSpawnCount = 2;
    [SerializeField]
    public Shield ShieldPrefab;

    [SerializeField]
    private int HorizontalCount = 3;

    [SerializeField]
    private int VerticalCount = 3;

    private const int BoardPlacementSize = 10;

    private BoardPlacement[,] placements;
       
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

                // @NOTE: Spawn unit every even tile player 1.
                if (i % 2 != 0 && j == 0)
                {
                    Unit unit = Instantiate(Player1UnitPrefab);                    
                    placement.SetUnit(unit, true);
                    unit.OwningAgent = AgentManager.Get().GetFirstAgent();
                }

                // @NOTE: Spawn unit every even tile for player 2.
                if (i % 2 != 0 && j == VerticalCount - 1)
                {
                    Unit unit = Instantiate(Player2UnitPrefab);
                    placement.SetUnit(unit, true);
                    unit.OwningAgent = AgentManager.Get().GetSecondAgent();
                }

                placements[i, j] = placement;
            }
        }

        int spawnedSwords = 0;
        int spawnedShields = 0;
        int spawnedPotoins = 0;

        for (int s = 0; s < 250; s++)
        {
            int i = Random.Range(0, HorizontalCount);
            int j = Random.Range(0, VerticalCount);
            int item = Random.Range(0, 3);
            float prob = Random.Range(0.0f, 1.0f) * 100.0f;
            if (placements[i, j].GetFirstUnit() == null && placements[i,j].item == null)
            {
                switch (item)
                {
                    case 0:
                        if (prob < SwordPrefab.SpawnProbablity && spawnedSwords < MaxSwordSpawnCount)
                        {
                            placements[i, j].item = Instantiate(SwordPrefab, placements[i, j].transform);
                            placements[i, j].item.transform.localPosition = new Vector3(0, 3, 0);
                            spawnedSwords++;
                        }
                        break;
                    case 1:
                        if (prob < ShieldPrefab.SpawnProbablity && spawnedShields < MaxShieldSpawnCount)
                        {
                            placements[i, j].item = Instantiate(ShieldPrefab, placements[i, j].transform);
                            placements[i, j].item.transform.localPosition = new Vector3(0, 1, 0);
                            spawnedShields++;
                        }
                        break;
                    case 2:
                        if (prob < PotionPrefab.SpawnProbablity && spawnedPotoins < MaxPotionSpawnCount)
                        {
                            placements[i, j].item = Instantiate(PotionPrefab, placements[i, j].transform);
                            placements[i, j].item.transform.localPosition = new Vector3(0, 1, 0);
                            spawnedPotoins++;
                        }
                        break;
                    default: Debug.LogError("You did not add the new item to the spawning code"); break;
                }
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
