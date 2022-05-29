using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TMP_Text TMP_hp;
    [SerializeField]
    int playertextlabel;
    //item pos in hieracrhy
    [SerializeField]
    public Transform WeaponVisualSpot = null;
    public GameItem Item { get; set; }
    public bool HasItem() => Item != null;
    public Agent OwningAgent { get; set; }
    public BoardPlacement CurrentPlacement { get; set; }

    public Unit(GameItem gameItem, BoardPlacement boardPlacement)
    {
        this.Item = gameItem;
    }

    private void Awake()
    {
        TMP_hp = GameObject.Find("Player " + $"{playertextlabel} HP").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        TMP_hp.text = HP.ToString();
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

            CurrentPlacement.RemoveUnit(this);
            path[0].SetUnit(this, true);
            //minuses moves remaining
            movesRemaining -= path.Count;

            return true;
        }
        else
        {
            Debug.Log("To far" + movesRemaining);
        }        

        return false;
    }

    public void EquipItem()
    {
        BoardPlacement placement = CurrentPlacement;

        if (!HasItem())
        {
            Item = placement.GetItem();
            placement.item = null;

            Item.AddBuff(this);

            //sets the parent and local postion so player will drag that itme around
            Item.transform.SetParent(WeaponVisualSpot, true);
            Item.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            // @NOTE: We have an item equiped so we need to swap the ours with the item on the floor

            Item.RemoveBuff(this);

            GameItem temp = placement.item;
            placement.item = Item;
            Item = temp;

            Item.AddBuff(this);

            placement.item.transform.SetParent(placement.transform);
            placement.item.transform.localPosition = new Vector3(0, 3, 0);

            Item.transform.SetParent(WeaponVisualSpot, true);
            Item.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void Heal()
    {
        CurrentPlacement.item.AddBuff(this);
        Destroy(CurrentPlacement.item.gameObject);
        CurrentPlacement.item = null;
    }

    public void DropItem()
    {
        CurrentPlacement.item = Item;
        Item = null;
        CurrentPlacement.item.transform.SetParent(CurrentPlacement.transform);
        CurrentPlacement.item.transform.localPosition = new Vector3(0, 3, 0);
    }

    public bool IsDead()
    {
        return HP <= 0;
    }

    public void Kill()
    {
        OwningAgent.RemoveUnit();
        CurrentPlacement.RemoveUnit(this);
        Destroy(this.gameObject);
    }

    public void Attack()
    {
        if (OwningAgent.CurrentMoves > 0)
        {
            // @NOTE: Get the enenmy unit if we have moves remaining;
            Unit enemyUnit = CurrentPlacement.GetFirstUnit() == this ? CurrentPlacement.GetSecondUnit() : CurrentPlacement.GetFirstUnit();
            // @NOTE: Calc the damange delt, the max is to make sure you can't deal negative damnage which thanks to maths would actually heal the enemy unit
            int dmg = Math.Max(ATK - enemyUnit.DEF, 0);
            enemyUnit.HP -= dmg;

            // NOTE: If he is dead remove him 
            if (enemyUnit.IsDead())
            {
                enemyUnit.Kill();
            }

            // @NOTE: Do some turn logic stuffs
            OwningAgent.CurrentMoves -= 1;
            OwningAgent.TurnLogic();
        }
    }

}
