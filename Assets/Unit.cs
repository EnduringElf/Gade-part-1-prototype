using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    public int MaxHP = 10;
    [SerializeField]
    public int HP = 10;
    [SerializeField]
    public int ATK = 2;
    [SerializeField]
    public int DEF = 1;

    //item pos in hieracrhy
    [SerializeField]
    public Transform WeaponVisualSpot = null;

    public GameItem Item { get; set; }
    public Agent OwningAgent { get; set; }
    public BoardPlacement CurrentPlacement { get; set; }


    private void Start()
    {
        
    }

    class PathFindingNode
    {
        public BoardPlacement p;
        public PathFindingNode back;
        public PathFindingNode(BoardPlacement p, PathFindingNode back) { this.p = p; this.back = back; }
    }
    public List<BoardPlacement> FindPath(BoardPlacement src, BoardPlacement dst)
    {      
        //using queues and hashsets to find niegbors to move to

        HashSet<BoardPlacement> explored = new HashSet<BoardPlacement>();

        Queue<PathFindingNode> q = new Queue<PathFindingNode>();
        q.Enqueue(new PathFindingNode(src, null));
        explored.Add(src);

        PathFindingNode end = null;
        while(q.Count > 0)
        {
            PathFindingNode p = q.Dequeue();
            if (p.p == dst)
            {
                end = p;
                break;
            }

            foreach (var n in p.p.neighbours)
            {
                if (!explored.Contains(n))
                {
                    PathFindingNode temp = new PathFindingNode(n, p);
                    q.Enqueue(temp);
                }
            }
        }

        List<BoardPlacement> path = new List<BoardPlacement>();
        if (end != null)
        {
            PathFindingNode c = end;
            while(c.back != null)
            {
                path.Add(c.p);
                c = c.back;
            }
        }

        return path;
    }

    // @NOTE: calculation works but if a player moves diagonally it cost both action points 
    // @NOTE: players can jump a tile so moveing works like a pawn in chess so pretty cool
    public bool MoveTo(BoardPlacement dest, ref int movesRemaining)
    {
        List<BoardPlacement> path = FindPath(CurrentPlacement, dest);

        //Debug.Log(path.Count);
        if (path.Count <= movesRemaining && path.Count > 0)
        {
            foreach (BoardPlacement p in path)
            {
                
            }

            CurrentPlacement.SetUnit(null);
            path[0].SetUnit(this, true);

            movesRemaining -= path.Count;

            return true;
        }
        else
        {
            Debug.Log("To far");
        }        

        return false;
    }

}
