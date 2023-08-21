using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class movePieces : MonoBehaviour
{
    public pieceLogic PieceLogic;
    public boardManager BoardManager;

    public float animationTime;

    int[,] indexBoard;
    int[,] currentPowBoard;
    int width;
    int height;

    bool canMove = true;

    public bool CalculateMove(Vector2Int direction)
    {
        //Debug.Log("Trying to move (calculateMove2)");
        if (!canMove)
        {
            Debug.Log("Cannot move: no possible moves left.");
            return false;
        }
        indexBoard = PieceLogic.indexBoard;
        currentPowBoard = (int[,])PieceLogic.powBoard.Clone();

        width = indexBoard.GetLength(0);
        height = indexBoard.GetLength(1);


        //Horizontal
        if (direction.y == 0)
        {
            for (int y = 0; y < height; y++)
            {
                //Here width is 1 if dir.x is negative, and width-2 if positive
                for (int x = (direction.x + 1) / 2 * (width - 1); x >= 0 && x < width; x -= direction.x)
                {
                    //If it has a valid index, move individual piece
                    if (indexBoard[x, y] >= 1 && indexBoard[x, y] <= PieceLogic.pieces.Count)
                    {
                        MoveIndividualPiece(indexBoard[x, y] - 1, direction);
                    }
                }
            }
        }
        //Vertical
        else
        {
            for (int x = 0; x < width; x++)
            {
                //Here width is 1 if dir.x is negative, and width-2 if positive
                for (int y = (direction.y + 1) / 2 * (height - 1); y >= 0 && y < height; y -= direction.y)
                {
                    //If it has a valid index, move individual piece
                    if (indexBoard[x, y] >= 1 && indexBoard[x, y] <= PieceLogic.pieces.Count)
                    {
                        MoveIndividualPiece(indexBoard[x, y] - 1, direction);
                    }

                }
            }
        }

        //Check if anything changed, if not, dont Reindex
        if (DidPowChange())
        {
            //Debug.Log("Moving finished, invoking..");
            Invoke("Contiunation", animationTime);
            canMove = CanMove();
            if (!canMove)
            {
                Debug.Log("---NO POSSIBLE MOVES LEFT---");
                return false;
            }
            return true;
        }
        else
        {
            Debug.Log("Cannot move in " + direction + " direction");
            return false;
        }
    }

    public bool CanMove()
    {
        if (PieceLogic.CountFreeSpace() == 0)
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (PieceLogic.powBoard[x, y] >= 1 && PieceLogic.powBoard[x, y] < 20)
                    {
                        //Check up
                        if (y < height - 1 && PieceLogic.powBoard[x, y] == PieceLogic.powBoard[x, y + 1])
                        {
                            Debug.Log(string.Format("Can move at {0};{1}", x, y));
                            return true;
                        }
                        //Check down
                        if (y > 1 && PieceLogic.powBoard[x, y] == PieceLogic.powBoard[x, y - 1])
                        {
                            Debug.Log(string.Format("Can move at {0};{1}", x, y));
                            return true;
                        }
                        //Check left
                        if (x > 1 && PieceLogic.powBoard[x, y] == PieceLogic.powBoard[x - 1, y])
                        {
                            Debug.Log(string.Format("Can move at {0};{1}", x, y));
                            return true;
                        }
                        //Check right
                        if (x < width - 1 && PieceLogic.powBoard[x, y] == PieceLogic.powBoard[x + 1, y])
                        {
                            Debug.Log(string.Format("Can move at {0};{1}", x, y));
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        else
            return true;
    }
    public bool DidPowChange()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                if (currentPowBoard[i, j] != PieceLogic.powBoard[i, j])
                    return true;
        }
        return false;
    }
    public void MoveIndividualPiece(int i, Vector2Int direction)
    {
        Vector2Int ogPos;
        Vector2Int moveToPos;
        pieceScript currentPieceScript;
        bool canMove;


        currentPieceScript = PieceLogic.pieces[i].GetComponent<pieceScript>();

        ogPos = currentPieceScript.gridPosition;
        moveToPos = ogPos;

        canMove = true;

        //Steps the target position until 
        //it hits an other piece or a wall

        while (canMove)
        {
            moveToPos += direction;
            //If out of bounds, cancel moving
            if (moveToPos.x < 0 || moveToPos.y < 0 || moveToPos.x >= width || moveToPos.y >= height)
                canMove = false;
            //If it not empty, or equal of the original power, cancel moving
            else if (currentPowBoard[moveToPos.x, moveToPos.y] != currentPowBoard[ogPos.x, ogPos.y] && currentPowBoard[moveToPos.x, moveToPos.y] != 0)
                canMove = false;

        }
        //Because we check the next position, we move one step back
        moveToPos -= direction;

        //Mark the starting position empty
        currentPowBoard[ogPos.x, ogPos.y] = 0;

        //If the current position is empty, simply copy its power
        if (currentPowBoard[moveToPos.x, moveToPos.y] == 0)
            currentPowBoard[moveToPos.x, moveToPos.y] = currentPieceScript.index;
        //If its not, mark the current position injoinable
        else
            currentPowBoard[moveToPos.x, moveToPos.y] = -1;

        //currentPieceScript.gridPosMoveTo = moveToPos;

        currentPieceScript.MoveTo(moveToPos, BoardManager.GetPositionFromGrid(moveToPos), animationTime);
    }
    public void Contiunation()
    {
        int playersOnGoal = PieceLogic.ReIndexBoard();
        //MiscFunc.LogInt2D(currentPowBoard);
        //Debug.Log("Players on goal " + playersOnGoal);
        BoardManager.CheckIfWin(playersOnGoal);
    }
}
