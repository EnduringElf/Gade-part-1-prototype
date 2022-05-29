using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAgent : MonoBehaviour
{

    public List<BoardPlacement> VBoard = new List<BoardPlacement>();

    public List<BoardPlacement> PossibleMoves;
    //Player 
    public Unit Player_1;
    //Ai
    public Unit Player_2;

    public void begin()
    {
        GetAllMoveableSpaces(Player_2.CurrentPlacement);
        GenerateTileWieghts();
        MiniMax(Player_2);
    }

    public void CreateTree()
    {

    }

    public int UtilityFinc()
    {
        if(Player_1.HP <= 0 || !Player_1) 
        {
            return -999;
        }
        else if( Player_2.HP <= 0 || !Player_2)
        {
            return 999;
        }
        else
        {

            int UtilFunc = 0;

            if(Player_2.Item.Name == "Sword") {UtilFunc += 30; } else { UtilFunc -= 30; }
            if (Player_2.Item.Name == "Shield"){ UtilFunc += 10;} else { UtilFunc -= 10; }
            if (Player_2.CurrentPlacement.GetItem().Name == "HP_pot") { UtilFunc += 20; } else { UtilFunc -= 20; }
            if (Player_2.Item.Name == "Sword" && Player_1.CurrentPlacement == Player_2.CurrentPlacement && Player_2.OwningAgent.CurrentMoves >= 1)  {  UtilFunc += 30; } else { UtilFunc -= 30; }
            
            return UtilFunc;
        }
    }

    public List<BoardPlacement> GetAllMoveableSpaces(BoardPlacement BP)
    {
        foreach(BoardPlacement boardPlacement in BP.neighbours)
        {
            foreach(BoardPlacement boardPlacement1 in boardPlacement.neighbours)
            {
                if (!PossibleMoves.Contains(boardPlacement1))
                {
                    boardPlacement1.Wieght = 1;
                    PossibleMoves.Add(boardPlacement1);
                }
            }
            if (!PossibleMoves.Contains(boardPlacement))
            {
                boardPlacement.Wieght = 2;
                PossibleMoves.Add(boardPlacement);
            }
        }
        return PossibleMoves;
    }

    public bool TerminalCheck(Unit player1, Unit player2)
    {
        if (player1.HP < 0|| !player1)
        {
            return false;
        }
        if(player2.HP < 0|| !player2)
        {
            return true;
        }

        return false;
    }

    public void GenerateTileWieghts()
    {
        if (VBoard != null)
        {
            foreach (BoardPlacement boardPlacement in VBoard)
            {
                if (boardPlacement.GetItem())
                {
                    if (boardPlacement.GetItem().Name == "Sword")
                    {
                        boardPlacement.Wieght += 3;
                    }
                    else if (boardPlacement.GetItem().Name == "HP_pot")
                    {
                        boardPlacement.Wieght += 2;
                    }
                    else if(boardPlacement.GetItem().Name == "HP_pot" && Player_2.HP > Player_1.HP)
                    {
                        boardPlacement.Wieght += 5;
                    }
                    else if (boardPlacement.GetItem().Name == "Shield")
                    {
                        boardPlacement.Wieght += 1;
                    }
                }
                if (boardPlacement.GetFirstUnit() && Player_2.Item.Name =="Sword")
                {
                    if(Player_1.Item.Name == "Sword" && Player_1.HP > Player_2.HP)
                    {
                        boardPlacement.Wieght += 6;
                    }
                    else
                    {
                        boardPlacement.Wieght -= 6;
                    }
                    
                }
                
            }
        }
        else
        {
            Debug.Log("Virtual board does not exist");
        }
        
    }

    public int MiniMax(Unit player)
    {
        int score;
        Unit OtherPlayer;
        BestMove(PossibleMoves);
        if (player == Player_1)
        {
             OtherPlayer = Player_2;
        }
        else
        {
             OtherPlayer = Player_1;
        }
        if(TerminalCheck(player, OtherPlayer))
        {
            score = 1;
            return score;
        }else if(TerminalCheck(OtherPlayer, player))
        {
            score = -1;
            return score ;
        }
        else
        {
            score = 0;
            return score;
        }
    }

    public void BestMove(List<BoardPlacement> boardPlacements)
    {
        int maxW = 0;
        int currentW;
        foreach(BoardPlacement bp in boardPlacements)
        {
            currentW = bp.Wieght;
            if(currentW > maxW)
            {
                maxW = currentW;
                Debug.Log(bp + " " + currentW );
            }
        }
    }
}
