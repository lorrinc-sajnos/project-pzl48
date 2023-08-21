using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boardManager : MonoBehaviour
{
    //GameplayLogic
    public GameplayLogic gameplayLogic;
    public LevelData levelData;
    public pieceLogic PieceLogic;
    public generateBG GenerateBG;

    //Interface var
    public Vector2 offset;
    public float horizontalUnitSize = 4;
    public float tileBaseUnitSize = 2;
    public Vector3 unitScale;

    public GameObject foregroundUI;

    //Not-public variables
    float pieceSize;
    float offsetX, offsetY;


    public void StartLevel()
    {
        VariableInit();

        GenerateBG = GetComponent<generateBG>();
        GenerateBG.GenerateBG();

        PieceLogic.VariableInit();
        PieceLogic.GenerateLevel();
    }

    public void RestartBoard()
    {
        PieceLogic.DeleteAllPieces();
        PieceLogic.GenerateLevel();
    }
    public void VariableInit()
    {
        levelData = gameplayLogic.levelData;

        if (unitScale.x == 0)
        {
            float memory = horizontalUnitSize / (levelData.boardSize.x * tileBaseUnitSize);
            unitScale = new Vector3(memory, memory, 0);
        }
        else
        {
            horizontalUnitSize = unitScale.x * (levelData.boardSize.x * tileBaseUnitSize);
        }

        pieceSize = unitScale.x * tileBaseUnitSize;
        offsetX = pieceSize / 2 - (horizontalUnitSize / 2) + offset.x;
        offsetY = pieceSize / 2 - (horizontalUnitSize / 2) + offset.y;
    }


    public Vector3 GetPositionFromGrid(Vector2 position)
    {
        return GetPositionFromGrid(position.x, position.y);
    }
    public Vector3 GetPositionFromGrid(float x, float y)
    {
        return new Vector3(offsetX + x * pieceSize, offsetY + y * pieceSize, 0);//* unitScale.x
    }

    public void CheckIfWin(int playerOnGoal)
    {
        //Debug.Log("Player count:"+levelData.playerCount);
        if (playerOnGoal == levelData.playerCount)
            gameplayLogic.DoWhenWin();
    }
}
