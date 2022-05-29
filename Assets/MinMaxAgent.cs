using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinMaxAgent : Agent
{
    public int SearchDepth = 3;
    public int MovementScore = 0;
    public int AttackScore = 1;
    public int PickupScore = 5;

    class StaticItem
    {
        public int ATK_Increase;
        public int DEF_Increase;
        public int HP_Restored;

        public StaticItem()
        {

        }

        public StaticItem(GameItem Item)
        {
            if (Item is Sword sword)
                ATK_Increase = sword.ATK_Increase;
            if (Item is Shield shield)
                DEF_Increase = shield.DEF_Increase;
            if (Item is HPPotion potion)
                HP_Restored = potion.HP_Restored;
        }

        public StaticItem Clone()
        {
            StaticItem item = new StaticItem();
            item.ATK_Increase = ATK_Increase;
            item.DEF_Increase = DEF_Increase;
            item.HP_Restored = HP_Restored;
            return item;
        }
    }

    class StaticUnit
    {
        public int HP;
        public int ATK;
        public int DEF;
        public bool homePlayer;
        public StaticItem item;

        public StaticUnit Clone()
        {
            StaticUnit unit = new StaticUnit();
            unit.HP = HP;
            unit.ATK = ATK;
            unit.DEF = DEF;
            unit.homePlayer = homePlayer;
            if (unit.item != null)
                unit.item = item.Clone();
            return unit;
        }
    }

    class StaticPlacement
    {
        public int i;
        public int j;
        public StaticUnit unit1;
        public StaticUnit unit2;
        public StaticItem item;

        public void PlaceUnit(StaticUnit unit)
        {
            if (unit1 == null)
                unit1 = unit;
            else if (unit2 == null)
                unit2 = unit;
        }

        public bool HasUnit(StaticUnit unit)
        {
            return unit1 == unit || unit2 == unit;
        }

        public StaticPlacement Clone()
        {
            StaticPlacement placement = new StaticPlacement();

            if (item != null)
                placement.item = item.Clone();

            if (unit1 != null)
                placement.unit1 = unit1.Clone();

            if (unit2 != null)
                placement.unit2  = unit2.Clone();

            placement.i = i;
            placement.j = j;

            return placement;
        }
    }

    class StaticMap
    {
        public int Verticel;
        public int Horizontal;
        public StaticUnit homeUnit;
        public StaticUnit enemyUnit;
        public StaticPlacement[,] placements;

        public StaticMap Clone()
        {
            StaticMap map = new StaticMap();
            map.Verticel = Verticel;
            map.Horizontal = Horizontal;
            map.placements = new StaticPlacement[Verticel, Horizontal];

            for (int i = 0; i < map.Verticel; i++)
            {
                for (int j = 0; j < map.Horizontal; j++)
                {
                    map.placements[i, j] = placements[i, j].Clone();

                    if (placements[i, j].unit1 == homeUnit)
                    {
                         map.homeUnit = map.placements[i, j].unit1;
                    }
                    if (placements[i, j].unit2 == homeUnit)
                    {
                        map.homeUnit = map.placements[i, j].unit2;
                    }

                    if (placements[i, j].unit1 == enemyUnit)
                    {
                        map.enemyUnit = map.placements[i, j].unit1;
                    }
                    if (placements[i, j].unit2 == enemyUnit)
                    {
                        map.enemyUnit = map.placements[i, j].unit2;
                    }
                }
            }

            return map;
        }
    }

    public override void Action(Map map)
    {
        StaticMap staticMap = new StaticMap();

        staticMap.Verticel = map.Verticle;
        staticMap.Horizontal = map.Horizontal;
        staticMap.placements = new StaticPlacement[map.Verticle, map.Horizontal];

        for (int i = 0; i < map.Verticle; i++)
        {
            for (int j = 0; j < map.Horizontal; j++)
            {
                StaticPlacement placement = new StaticPlacement();
                placement.i = i;
                placement.j = j;

                if (map.placements[i, j].GetFirstUnit() != null)
                {
                    Unit runit = map.placements[i, j].GetFirstUnit();

                    StaticUnit unit = new StaticUnit();
                    unit.ATK = runit.ATK;
                    unit.DEF = runit.DEF;
                    unit.HP = runit.HP;
                    unit.homePlayer = runit.OwningAgent == this;

                    if (unit.homePlayer)
                        staticMap.homeUnit = unit;

                    if (!unit.homePlayer)
                        staticMap.enemyUnit = unit;

                    if (runit.HasItem())
                    {
                        unit.item = new StaticItem(runit.Item);
                    }

                    placement.unit1 = unit;
                }

                if (map.placements[i, j].GetSecondUnit() != null)
                {
                    Unit runit = map.placements[i, j].GetSecondUnit();

                    StaticUnit unit = new StaticUnit();
                    unit.ATK = runit.ATK;
                    unit.DEF = runit.DEF;
                    unit.HP = runit.HP;
                    unit.homePlayer = runit.OwningAgent == this;

                    if (unit.homePlayer)
                        staticMap.homeUnit = unit;

                    if (!unit.homePlayer)
                        staticMap.enemyUnit = unit;

                    if (runit.HasItem())
                    {
                        unit.item = new StaticItem(runit.Item);
                    }

                    placement.unit2 = unit;
                }

                if (map.placements[i, j].item != null)
                    placement.item = new StaticItem(map.placements[i, j].item);

                staticMap.placements[i, j] = placement;
            }
        }
        

        Move[] moves = GetMoves(staticMap, null, staticMap.homeUnit);

        Move move = MiniMax(0, true, moves, staticMap);
        move.picked = true;
        while (move.lastMove != null)
        {
            move = move.lastMove;
            move.picked = true;
        }

        Debug.Log("AI decided to " + move.type);



        switch (move.type)
        {
            case MoveType.MOVELEFT:
                { 
                    BoardPlacement placement = map.placements[unit.CurrentPlacement.i - 1, unit.CurrentPlacement.j];
                    unit.MoveTo(placement, ref CurrentMoves);


                    if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                        OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                }

                break;
            case MoveType.MOVERIGHT:
                {
                    BoardPlacement placement = map.placements[unit.CurrentPlacement.i + 1, unit.CurrentPlacement.j];
                    unit.MoveTo(placement, ref CurrentMoves);

                    if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                        OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                }
                break;
            case MoveType.MOVEDOWN:
                {
                    BoardPlacement placement = map.placements[unit.CurrentPlacement.i, unit.CurrentPlacement.j - 1];
                    unit.MoveTo(placement, ref CurrentMoves);


                    if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                        OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                }
                break;
            case MoveType.MOVEUP:
                {
                    BoardPlacement placement = map.placements[unit.CurrentPlacement.i, unit.CurrentPlacement.j + 1];
                    unit.MoveTo(placement, ref CurrentMoves);

                    if (placement.GetFirstUnit() != null && placement.GetSecondUnit() != null)
                        OffsetUnits(placement.GetFirstUnit(), placement.GetSecondUnit());
                }
                break;
            case MoveType.ATTACK:
                unit.Attack();
                break;
            case MoveType.PICKUP:
                if (unit.CurrentPlacement.item is HPPotion)
                {
                    unit.Heal();
                }
                else
                {
                    unit.EquipItem();
                }
                break;
        }

        AgentManager.Get().SwapTurns();
    }

    void PrintMoves(Move[] moves, bool max)
    {
        Debug.Log("============================");
        foreach (Move move in moves)
        {
            string msg = move.type + " " + move.score + " " + (max ? "max" : "min");
            Debug.Log(msg);
        }
        Debug.Log("============================");
    }

    enum MoveType
    {
        MOVELEFT,
        MOVERIGHT,
        MOVEDOWN,
        MOVEUP,
        ATTACK,
        PICKUP
    }
      
    class Move
    {
        public MoveType type;
        public int score;
        public Move lastMove;
        public bool picked;

        public void DoMove(StaticMap map, StaticUnit unit)
        {
            StaticPlacement placement = null;
            for (int i = 0; i < map.Verticel; i++)
            {
                for (int j = 0; j < map.Horizontal; j++)
                {
                    if (map.placements[i,j].HasUnit(unit))
                        placement = map.placements[i, j];
                }
            }

            switch (type)
            {
                case MoveType.MOVELEFT:
                    { 
                        if (placement.unit1 == unit)
                        {
                            placement.unit1 = null;
                            map.placements[placement.i - 1, placement.j].PlaceUnit(unit);
                        }
                        else
                        {
                            placement.unit2 = null;
                            map.placements[placement.i - 1, placement.j].PlaceUnit(unit);
                        }
                    }break;
                case MoveType.MOVERIGHT:
                    if (placement.unit1 == unit)
                    {
                        placement.unit1 = null;
                        map.placements[placement.i + 1, placement.j].PlaceUnit(unit);
                    }
                    else
                    {
                        placement.unit2 = null;
                        map.placements[placement.i + 1, placement.j].PlaceUnit(unit);
                    }
                    break;
                case MoveType.MOVEDOWN:
                    if (placement.unit1 == unit)
                    {
                        placement.unit1 = null;
                        map.placements[placement.i , placement.j - 1].PlaceUnit(unit);
                    }
                    else
                    {
                        placement.unit2 = null;
                        map.placements[placement.i , placement.j - 1].PlaceUnit(unit);
                    }
                    break;
                case MoveType.MOVEUP:
                    if (placement.unit1 == unit)
                    {
                        placement.unit1 = null;
                        map.placements[placement.i, placement.j + 1].PlaceUnit(unit);
                    }
                    else
                    {
                        placement.unit2 = null;
                        map.placements[placement.i, placement.j + 1].PlaceUnit(unit);
                    }
                    break;
                case MoveType.ATTACK:
                    {
                        if (placement.unit1 == unit)
                        {
                            placement.unit2.HP -= Mathf.Max(unit.ATK - placement.unit2.DEF, 0);
                        }
                        else
                        {
                            placement.unit1.HP -= Mathf.Max(unit.ATK - placement.unit1.DEF, 0);
                        }
                    }
                    break;
                case MoveType.PICKUP:
                    { 
                        unit.item = placement.item;
                        unit.ATK += placement.item.ATK_Increase;
                        unit.DEF += placement.item.DEF_Increase;
                        unit.HP += placement.item.HP_Restored;
                        placement.item = null;                 
                    }
                    break;
            }
        }
    }

    private Move[] GetMoves(StaticMap map, Move lastMove, StaticUnit unit)
    {
        List<Move> moves = new List<Move>();

        
        for (int i = 0; i < map.Verticel; i++)
        {
            for (int j = 0; j < map.Horizontal; j++)
            {
                StaticPlacement placement = map.placements[i, j];
                if  (placement.unit1 != null && placement.unit2 != null)
                {
                    Move move = new Move();
                    move.type = MoveType.ATTACK;
                    move.score = AttackScore;
                    moves.Add(move);
                }

                if (placement.item != null && unit.item == null && (placement.unit1 == unit || placement.unit2 == unit))
                {
                    Move move = new Move();
                    move.type = MoveType.PICKUP;
                    move.score = PickupScore;
                    moves.Add(move);
                }

                if (placement.unit1 == unit || placement.unit2 == unit)
                {
                    if (i - 1 >= 0)
                    {
                        Move move = new Move();
                        move.type = MoveType.MOVELEFT;
                        move.score = MovementScore;
                        moves.Add(move);
                    }

                    if (i + 1 < map.Verticel)
                    {
                        Move move = new Move();
                        move.type = MoveType.MOVERIGHT;
                        move.score = MovementScore;
                        moves.Add(move);
                    }

                    if (j - 1 >= 0)
                    {
                        Move move = new Move();
                        move.type = MoveType.MOVEDOWN;
                        move.score = MovementScore;
                        moves.Add(move);
                    }

                    if (j + 1 < map.Horizontal)
                    {
                        Move move = new Move();
                        move.type = MoveType.MOVEUP;
                        move.score = MovementScore;
                        moves.Add(move);
                    }
                }
            }
        }

        for (int i = 0; i < moves.Count; i++)
        {
            if (lastMove != null)
            {
                moves[i].lastMove = lastMove;
            }            
        }

        return moves.ToArray();
    }

    private Move FindMaxMove(Move[] moves)
    {
        Move move = moves[0];
        for (int i = 1; i < moves.Length; i++)
            if (moves[i].score > move.score)
                move = moves[i];

        return move;
    }

    private Move FindMinMove(Move[] moves)
    {
        Move move = moves[0];
        for (int i = 1; i < moves.Length; i++)
            if (moves[i].score < move.score)
                move = moves[i];

        return move;
    }

    private Move MiniMax(int depth, bool isMax, Move[] moves, StaticMap map)
    {
        if (depth == SearchDepth)
        {
            Move move;
            if (isMax)
            {
                move = FindMinMove(moves);
            }
            else
            {
                move = FindMaxMove(moves);
            }
          
            return move;
        }

        if (isMax)
        {
            List<Move> list = new List<Move>();
            foreach(Move move in moves)
            {
                StaticMap newMap = map.Clone();
                move.DoMove(newMap, newMap.homeUnit);
                Move[] newMoves = GetMoves(newMap, move, newMap.enemyUnit);
                list.Add(MiniMax(depth + 1, false, newMoves, newMap));
            }

            return FindMaxMove(list.ToArray());
        }
        else
        {
            List<Move> list = new List<Move>();
            foreach (Move move in moves)
            {
                StaticMap newMap = map.Clone();
                move.DoMove(newMap, newMap.enemyUnit);
                Move[] newMoves = GetMoves(newMap, move, newMap.homeUnit);
                list.Add(MiniMax(depth + 1, true, newMoves, newMap));
            }
          
            return FindMinMove(list.ToArray());
        }
    }

    //public List<BoardPlacement> VBoard;

    //public List<BoardPlacement> PossibleMoves;
    //public Unit Player_1;
    //public Unit Player_2;

    //public void begin()
    //{
    //    GetAllMoveableSpaces(Player_2.CurrentPlacement);
    //    GenerateTileWieghts();
    //    MiniMax(Player_2);
    //}

    //public void Prediction()
    //{
    //    foreach(BoardPlacement boardPlacement in PossibleMoves)
    //    {

    //    }
    //}

    //public int UtilityFinc()
    //{
    //    if(Player_1.HP <= 0 || !Player_1) 
    //    {
    //        return -999;
    //    }
    //    else if( Player_2.HP <= 0 || !Player_2)
    //    {
    //        return 999;
    //    }
    //    else
    //    {

    //        int UtilFunc = 0;

    //        if(Player_2.Item.Name == "Sword") {UtilFunc += 30; } else { UtilFunc -= 30; }
    //        if (Player_2.Item.Name == "Shield"){ UtilFunc += 10;} else { UtilFunc -= 10; }
    //        if (Player_2.CurrentPlacement.GetItem().Name == "HP_pot") { UtilFunc += 20; } else { UtilFunc -= 20; }
    //        if (Player_2.Item.Name == "Sword" && Player_1.CurrentPlacement == Player_2.CurrentPlacement && Player_2.OwningAgent.CurrentMoves >= 1)  {  UtilFunc += 30; } else { UtilFunc -= 30; }

    //        return UtilFunc;
    //    }
    //}

    //public List<BoardPlacement> GetAllMoveableSpaces(BoardPlacement BP)
    //{
    //    foreach(BoardPlacement boardPlacement in BP.neighbours)
    //    {
    //        foreach(BoardPlacement boardPlacement1 in boardPlacement.neighbours)
    //        {
    //            if (!PossibleMoves.Contains(boardPlacement1))
    //            {
    //                boardPlacement1.Wieght = 1;
    //                //defualt wieght for far away spaces
    //                PossibleMoves.Add(boardPlacement1);
    //            }
    //        }
    //        if (!PossibleMoves.Contains(boardPlacement))
    //        {
    //            boardPlacement.Wieght = 2;
    //            //defualt wieght for close spaces
    //            PossibleMoves.Add(boardPlacement);
    //        }
    //    }
    //    return PossibleMoves;
    //}

    //public bool TerminalCheck(Unit player1, Unit player2)
    //{
    //    if (player1.HP < 0|| !player1)
    //    {
    //        return false;
    //    }
    //    if(player2.HP < 0|| !player2)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //public void GenerateTileWieghts()
    //{
    //    if (VBoard != null)
    //    {
    //        foreach (BoardPlacement boardPlacement in VBoard)
    //        {
    //            if (boardPlacement.GetItem())
    //            {
    //                if (boardPlacement.GetItem().Name == "Sword")
    //                {
    //                    boardPlacement.Wieght += 3;
    //                }
    //                else if (boardPlacement.GetItem().Name == "HP_pot")
    //                {
    //                    boardPlacement.Wieght += 2;
    //                }
    //                else if(boardPlacement.GetItem().Name == "HP_pot" && Player_2.HP > Player_1.HP)
    //                {
    //                    boardPlacement.Wieght += 5;
    //                    //prioritise hp over fighting
    //                }
    //                else if (boardPlacement.GetItem().Name == "Shield")
    //                {
    //                    boardPlacement.Wieght += 1;
    //                }
    //            }
    //            if (boardPlacement.GetFirstUnit() && Player_2.Item.Name =="Sword")
    //            {
    //                if(Player_1.Item.Name == "Sword" && Player_1.HP > Player_2.HP)
    //                {
    //                    boardPlacement.Wieght += 6;
    //                    //leads to attacking
    //                }
    //                else
    //                {
    //                    boardPlacement.Wieght -= 6;
    //                    //leads to finding health or rather avoiding the players position
    //                }

    //            }

    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("Virtual board does not exist");
    //    }

    //}

    //public int MiniMax(Unit player)
    //{
    //    int score;
    //    Unit OtherPlayer;
    //    BestMove(PossibleMoves);
    //    if (player == Player_1)
    //    {
    //         OtherPlayer = Player_2;
    //    }
    //    else
    //    {
    //         OtherPlayer = Player_1;
    //    }
    //    if(TerminalCheck(player, OtherPlayer))
    //    {
    //        score = 1;
    //        return score;
    //    }else if(TerminalCheck(OtherPlayer, player))
    //    {
    //        score = -1;
    //        return score ;
    //    }
    //    else
    //    {
    //        score = 0;
    //        return score;
    //    }
    //}

    //public void BestMove(List<BoardPlacement> boardPlacements)
    //{
    //    int maxW = 0;
    //    int currentW;

    //    foreach(BoardPlacement bp in boardPlacements)
    //    {
    //        currentW = bp.Wieght;
    //        if(currentW > maxW)
    //        {
    //            maxW = currentW;
    //            Debug.Log(bp + " " + currentW );
    //        }
    //    }
    //}


}
