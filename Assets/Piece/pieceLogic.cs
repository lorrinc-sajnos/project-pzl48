using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceLogic : MonoBehaviour
{
    public boardManager BoardManager;
    public GameObject piece;

    LevelData levelData;

    //LOGIC variables
    public int maxPowFound = -1;
    public int maxPowLvl = 12;


    public int howMuchRandom;
    public int[] choosablePow;


    public int[,] indexBoard;
    public int[,] powBoard;
    public List<GameObject> pieces;

    int[,] indexBoardClean;

    // Start is called before the first frame update
    public void VariableInit()
    {
        levelData = BoardManager.levelData;

        pieces = new List<GameObject>();

        //Initializing the index 2d array
        indexBoardClean = new int[levelData.boardSize.x, levelData.boardSize.y];

        for (int i = 0; i < levelData.boardSize.x; i++)
        {
            for (int j = 0; j < levelData.boardSize.y; j++)
            {
                if (levelData.startSetup[i, j] == 255)
                    indexBoardClean[i, j] = 255;
            }
        }
        indexBoard = (int[,])indexBoardClean.Clone();
        powBoard = (int[,])indexBoardClean.Clone();
    }

    public void GenerateLevel()
    {
        for (int i = 0; i < levelData.boardSize.x; i++)
        {
            for (int j = 0; j < levelData.boardSize.y; j++)
            {
                //If the number is between 1 and maxPowLvl, that means it's a 2^x piece
                if (levelData.startSetup[i, j] >= 1 && levelData.startSetup[i, j]!=255) //&& LevelData.startSetup[i, j] <= maxPowLvl)
                {
                    CreatePiece(new Vector2Int(i, j), levelData.startSetup[i, j]);
                };
            }
        }
        ReIndexBoard();
    }

    public void DeleteAllPieces()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            Destroy(pieces[i]);
        }
        pieces = new List<GameObject>();
    }

    /// <summary>
    /// Re indexes the board, and returns the amount of players that on a goal
    /// </summary>
    /// <returns>Returns the amount of players that on a goal</returns>
    public int ReIndexBoard()
    {
        int playerOnGoalCount = 0;

        indexBoard = (int[,])indexBoardClean.Clone();
        powBoard = (int[,])indexBoardClean.Clone();

        pieceScript mem;
        Vector2Int pos;
        for (int i = 0; i < pieces.Count; i++)
        {
            //Debug.Log("Piece" + (i + 1) + "/" + pieces.Count);
            mem = pieces[i].GetComponent<pieceScript>();
            pos = mem.gridPosition;

            //If the indexed place is empty, place its index there
            if (indexBoard[pos.x, pos.y] == 0)
            {
                indexBoard[pos.x, pos.y] = i + 1;

                if (maxPowFound < mem.index)
                    maxPowFound = mem.index;

                powBoard[pos.x, pos.y] = mem.index;
                ///*
                if (mem.index == 20 && levelData.isGoal[pos.x, pos.y])
                {
                    playerOnGoalCount++;
                    mem.isHappy = true;
                    mem.UpdateTexture();
                }
                //*/
            }
            //If it isn't, that means that two pieces merged
            else if (indexBoard[pos.x, pos.y] >= 1 && indexBoard[pos.x, pos.y] <= pieces.Count)
            {
                //Delete this one from scene and list
                Destroy(pieces[i]);
                pieces.RemoveAt(i);

                //Rasie the power of the one on the board
                mem = pieces[indexBoard[pos.x, pos.y] - 1].GetComponent<pieceScript>();
                mem.RaisePower(1);

                //Debug.Log("Reindexed board!");
                return ReIndexBoard();
            }
        }
        if (levelData.generateRandom)
        {
            //Debug.Log("Adding random pieces!");
            AddRandom(howMuchRandom, choosablePow);
        }

        //Debug.Log("Done!");
        return playerOnGoalCount;
    }

    public void AddRandom(int number, int[] choosableNum)
    {
        //Gets how many free spaces are
        int freeSpace = CountFreeSpace();

        //The n th free space to place the piece
        int index;
        int counter;
        int pow;
        //Then loop around, until the N th free space is reached
        for (int i = 0; i < number && freeSpace > 0; i++)
        {
            //Debug.Log("Num. of random added " + (i + 1) + "/" + number);
            index = Random.Range(0, freeSpace);
            //Debug.Log("Chosen index:" + index);
            pow = choosableNum[Random.Range(0, choosableNum.Length)];
            counter = 0;

            for (int x = 0; x < levelData.boardSize.x && counter <= index; x++)
            {
                for (int y = 0; y < levelData.boardSize.y; y++)
                {
                    if (powBoard[x, y] == 0)
                    {
                        if (index == counter)
                        {
                            //Debug.Log("Creating random piece " + (i + 1) + "/" + number + ", counter: " + counter);
                            CreatePiece(new Vector2Int(x, y), pow);
                            indexBoard[x, y] = pieces.Count;
                            powBoard[x, y] = pow;
                            counter++;
                            break;
                        }
                        else
                            counter++;
                    }
                }
            }
            freeSpace--;
        }

    }

    public int CountFreeSpace()
    {
        int count = 0;

        for (int i = 0; i < levelData.boardSize.x; i++)
        {
            for (int j = 0; j < levelData.boardSize.y; j++)
            {
                if (powBoard[i, j] == 0)
                    count++;
            }
        }

        return count;
    }
    public void CreatePiece(Vector2Int gridPosition, int index)
    {
        GameObject tempObj = Instantiate(piece);

        tempObj.layer = 10;
        tempObj.transform.position = BoardManager.GetPositionFromGrid(gridPosition);
        tempObj.transform.localScale = BoardManager.unitScale;
        pieceScript PieceScript = tempObj.GetComponent<pieceScript>();

        PieceScript.gridPosition = gridPosition;
        PieceScript.index = index;

        pieces.Add(tempObj);
    }
}
