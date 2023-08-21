using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorData : MonoBehaviour
{
    public EditorGenerateGrid editorGenerateGrid;

    public InputField levelNameInp;
    public InputField levelAuthorInp;
    public InputField gridSizeXInp;

    public InputField levelBase64Output;


    public Vector2Int gridSize;
    public List<Vector2Int> moves;
    public GameObject[,] gameObjGrid;
    public string selectedButton = "";
    public int currentSelectedIndex = 1;

    public string levelBase64;

    // Start is called before the first frame update
    void Start()
    {
        if (GlobalData.isLevelEditorMode)
        {
            GlobalData.isLevelEditorMode = false;
            EditorFromLevelData(GlobalData.levelToLoad);
        }
    }
    public void GenerateNewGrid()
    {
        if (gridSizeXInp.text == "")
            return;

        gridSize = new Vector2Int(Convert.ToInt32(gridSizeXInp.text), Convert.ToInt32(gridSizeXInp.text));

        if (gameObjGrid != null)
        {
            foreach (GameObject G in gameObjGrid)
                Destroy(G);
        }
        gameObjGrid = new GameObject[gridSize.x, gridSize.y];

        //editorGenerateGrid.ResizeBG();
        editorGenerateGrid.VarInit();
        editorGenerateGrid.GenerateGrid();
    }

    public LevelData GenerateLevelData(int levelEncoding)
    {
        LevelData levelData = new LevelData();

        levelData.levelVersion = levelEncoding;

        levelData.levelName = levelNameInp.text;
        levelData.authorName = levelAuthorInp.text;

        levelData.boardSize = gridSize;

        levelData.solutionCount = moves.Count;
        levelData.solutionSwipes = moves.ToArray();

        levelData.startSetup = new int[gridSize.x, gridSize.y];
        levelData.isGoal = new bool[gridSize.x, gridSize.y];

        levelData.goalCount = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                EditorPieceScript temp = gameObjGrid[x, y].GetComponent<EditorPieceScript>();

                if (temp.hasGoal)
                {
                    levelData.isGoal[x, y] = true;
                    levelData.goalCount++;
                }

                levelData.startSetup[x, y] = temp.index;
            }
        }
        levelData.VariableInit();
        return levelData;
    }

    public void EditorFromLevelData(LevelData levelData)
    {
        levelNameInp.text = levelData.levelName;
        levelAuthorInp.text = levelData.authorName;
        gridSize = levelData.boardSize;

        gridSizeXInp.text = "" + levelData.boardSize.x;

        moves = GlobalData.moves;

        GenerateNewGrid();

        EditorPieceScript temp;
        for (int i = 0; i < levelData.boardSize.x; i++)
        {
            for (int j = 0; j < levelData.boardSize.y; j++)
            {
                //Imitate button presses
                temp = gameObjGrid[i, j].GetComponent<EditorPieceScript>();
                temp.VarInit();

                temp.hasGoal = levelData.isGoal[i, j];
                temp.isTwoPow = levelData.startSetup[i, j] >= 1 && levelData.startSetup[i, j] < 20;
                temp.index = levelData.startSetup[i, j];

                temp.UpdateTexture();
            }
        }
    }

    public void GenerateBase64Code(int levelEncoding)
    {
        LevelData levelToSave = GenerateLevelData(levelEncoding);
        levelBase64Output.text = levelToSave.ToBase64();
    }

    public void Back()
    {
        GlobalData.BackToMenu("customLvlSelect");
    }
}
