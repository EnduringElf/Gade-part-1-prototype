using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleintSub : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 10;
    private int HP = 10;
    [SerializeField]
    private int ATK = 2;
    [SerializeField]
    private int DEF = 1;
    [SerializeField]
    private int MoveRate = 2;
    [SerializeField]
    private ItemSub Weapon;
    [SerializeField]
    private TempBuffSub Buff;
    [SerializeField]
    private int Buff_Duration = 0;

    public Agent OwningAgent { get; set; }
    public int MovesRemaining { get; set; }
    public BoardPlacement CurrentPlacement { get; set; }

    //passing through weapons from board to charchters
    public void Addweapon(ItemSub item)
    {
        Weapon = item;
        ItemAdj(Weapon);
    }

    public void AddBuff(TempBuffSub TBuff)
    {
        Buff = TBuff;
        Buffadd(Buff);
    }

    private void Buffadd(TempBuffSub t)
    {
        ATK += t.ATK;
        DEF += t.DEF;
        maxHP += t.HP;
        Heal(t.HP);
        Buff_Duration = t.duration;
    }
    private void Buffminus()
    {
        ATK -= Buff.ATK;
        DEF -= Buff.DEF;
        maxHP -= Buff.HP;
    }
    private void Heal(int value)
    {
        for(int i = 0; i< value; i++)
        {
            if(HP !>= maxHP)
            {
                HP++;
            }
            else
            {

            }
        }
    }

    //finding adjustment change to stats
    private void ItemAdj(ItemSub item)
    {
        ATK += item.ATK;
        DEF += item.DEF;
        HP += item.HP;
    }

    private void Attack(CleintSub enemny)
    {
        enemny.HP -= ATK;
    }


    class Node
    {
        public BoardPlacement p;
        public Node back;
        public Node(BoardPlacement p, Node back) { this.p = p; this.back = back; }
    }

    public List<BoardPlacement> FindPath(BoardPlacement src, BoardPlacement dst)
    {       
        HashSet<BoardPlacement> explored = new HashSet<BoardPlacement>();

        Queue<Node> q = new Queue<Node>();
        q.Enqueue(new Node(src, null));
        explored.Add(src);

        Node end = null;
        while(q.Count > 0)
        {
            Node p = q.Dequeue();
            if (p.p == dst)
            {
                end = p;
                break;
            }

            foreach (var n in p.p.neighbours)
            {
                if (!explored.Contains(n))
                {
                    Node temp = new Node(n, p);
                    q.Enqueue(temp);
                }
            }
        }

        List<BoardPlacement> path = new List<BoardPlacement>();
        if (end != null)
        {
            Node c = end;
            while(c.back != null)
            {
                path.Add(c.p);
                c = c.back;
            }
        }

        return path;
    }

    // @NOTE: 
    public bool MoveTo(BoardPlacement dest)
    {
        List<BoardPlacement> path = FindPath(CurrentPlacement, dest);

        Debug.Log(path.Count);
        if (path.Count <= MovesRemaining && path.Count > 0)
        {
            foreach (BoardPlacement p in path)
            {
                
            }

            transform.position = path[0].transform.position;
            MovesRemaining -= path.Count;
        }
        else
        {
            Debug.Log("To far");
        }        

        return false;
    }

}
