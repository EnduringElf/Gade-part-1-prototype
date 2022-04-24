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
    public HPPotion PotionPrefab;

    [SerializeField]
    public int MaxShieldSpawnCount = 2;

    [SerializeField]
    public Shield ShieldPrefab;

    [SerializeField]
    private int Verticle = 3;

    [SerializeField]
    private int Horizontal = 3;

    private const int BoardPlacementSize = 10;

    private BoardPlacement[,] placements;

    
    
       
    void Start()
    {

        GameObject board = new GameObject();
        placements = new BoardPlacement[Verticle, Horizontal];

        board.transform.name = "Board";        
                
        for (int i = 0; i < Verticle; i++)
        { 
            for (int j = 0; j < Horizontal; j++)
            {
                BoardPlacement placement = Instantiate(BoardPlacementPrefab);
                placement.transform.position = new Vector3(i, 0, j) * BoardPlacementSize - 
                    new Vector3(BoardPlacementSize * (Verticle / 2), 0, BoardPlacementSize * (Horizontal / 2));

                placement.name = "Board placement " + $"{i}, {j}";
                placement.transform.SetParent(board.transform);

                //spawn player 1 at middle of the board
                if (i == Verticle-(Verticle/2) -1 && j == Horizontal-(Horizontal/2)-1)
                {
                    Unit unit = Instantiate(Player1UnitPrefab);
                    placement.SetUnit(unit, true);
                    unit.OwningAgent = AgentManager.Get().GetFirstAgent();
                }

                //spawn player 1 at middle of the board
                if (i == Verticle - (Verticle / 2) - 1 && j == Horizontal - (Horizontal / 2)-1)
                {
                    Unit unit = Instantiate(Player2UnitPrefab);
                    placement.SetUnit(unit, true);
                    unit.OwningAgent = AgentManager.Get().GetSecondAgent();
                }

                    placements[i, j] = placement;
            }
        }
        //code for spawning items

        int spawnedSwords = 0;
        int spawnedShields = 0;
        int spawnedPotoins = 0;

        for (int s = 0; s < 250; s++)
        {
            int i = Random.Range(0, Verticle);
            int j = Random.Range(0, Horizontal);
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
        //code for nieghbor system
        for (int i = 0; i < Verticle; i++)
        {
            for (int j = 0; j < Horizontal; j++)
            {
                if (i == 0 && j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                }
                else if (i == Verticle - 1 && j == Horizontal - 1)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                }
                else if (i == 0 && j == Horizontal - 1)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                }
                else if (i == Verticle - 1 && j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                }
                else if (i == 0)
                {
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    placements[i, j].neighbours.Add(placements[i , j -1]);
                    //Debug.Log("placement last i, j: " + $"{i},{j} this is along the left side");
                }
                else if (i == Verticle - 1)
                {
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    //Debug.Log("placement last i, j: " + $"{i},{j} this is along the right side");
                }
                else if (j == 0)
                {
                    placements[i, j].neighbours.Add(placements[i, j + 1]);
                    placements[i, j].neighbours.Add(placements[i + 1, j]);
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    //Debug.Log("placement last i, j: " + $"{i},{j}this is along the bottom");
                }
                else if (j == Horizontal - 1)
                {
                    placements[i, j].neighbours.Add(placements[i, j - 1]);
                    placements[i, j].neighbours.Add(placements[i - 1, j]);
                    placements[i, j].neighbours.Add(placements[i  + 1, j]);
                    //Debug.Log("placement last i, j: " + $"{i},{j} this is along the top");
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
        //recayst to hot on mouse down
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit info;
            //if ray hot the board 
            if (Physics.Raycast(ray, out info, LayerMask.GetMask("Board")))
            {
                //sets the placement to boardplacement info from ray
                
                BoardPlacement placement = info.collider.gameObject.GetComponent<BoardPlacement>();

                //set the agent postion to a place on the board
                if (placement)
                {
                    
                    AgentManager.Get().GetCurrentAgent().Action(placement);
                    //ButtonLogic.Get().Set(placement);

                }
            }          
        }
    }
}
