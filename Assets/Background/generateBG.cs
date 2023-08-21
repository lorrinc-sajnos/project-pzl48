using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;

public class generateBG : MonoBehaviour
{
    //GScript refference

    public boardManager BoardManager;
    LevelData LevelData;

    public Sprite GoalTexture;

    public Color tileColor;
    public Sprite tileTexture;

    public Color backgroundColor;
    public Sprite BackgroundBase;

    public void GenerateBG()
    {
        //BoardManager = GetComponent<boardManager>();
        LevelData = BoardManager.levelData;
        int xSize = LevelData.boardSize.x;
        int ySize = LevelData.boardSize.y;

        //Debug.Log(xSize+" "+ ySize);



        //Calculate unitScale
        GameObject currentObj;
        SpriteRenderer currentSpriteRenderer;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (LevelData.startSetup[x, y] != 255)
                {
                    currentObj = new GameObject(string.Format("Board tile {0};{1}", x, y));

                    currentObj.AddComponent<SpriteRenderer>();
                    currentSpriteRenderer = currentObj.GetComponent<SpriteRenderer>();

                    currentSpriteRenderer.sortingLayerName = "Background";

                    if (LevelData.isGoal[x, y])
                    {
                        currentSpriteRenderer.sortingOrder = 2;

                        currentSpriteRenderer.sprite = GoalTexture;
                    }
                    else
                    {
                        currentSpriteRenderer.sortingOrder = 1;

                        currentSpriteRenderer.sprite = tileTexture;
                        currentSpriteRenderer.color = tileColor;
                    }

                    currentObj.layer = 9;
                    currentObj.transform.position = BoardManager.GetPositionFromGrid(x, y);
                    currentObj.transform.localScale = BoardManager.unitScale;
                }
            }
        }

        GenerateBackgroundBase();
        GenerateForegroundBase();
    }

    void GenerateBackgroundBase()
    {
        //Background base
        //Debug.Log("Generating bg base!");

        GameObject currentObj = new GameObject(string.Format("BackgroundBase", 0, 0));

        currentObj.AddComponent<SpriteRenderer>();
        SpriteRenderer currentSpriteRenderer = currentObj.GetComponent<SpriteRenderer>();
        currentSpriteRenderer.sprite = BackgroundBase;
        currentSpriteRenderer.color = backgroundColor;
        currentSpriteRenderer.sortingLayerName = "Background";
        currentSpriteRenderer.sortingOrder = 0;

        currentObj.transform.position = new Vector3(BoardManager.offset.x, BoardManager.offset.y, 0);
        float bgScale = BoardManager.horizontalUnitSize / 8f;
        currentObj.transform.localScale = new Vector3(bgScale, bgScale, 0);
        //foregroundUI
    }

    void GenerateForegroundBase()
    {
        BoardManager.foregroundUI = new GameObject("Foreground UI thingy");

        BoardManager.foregroundUI.AddComponent<SpriteRenderer>();
        SpriteRenderer currentSpriteRenderer = BoardManager.foregroundUI.GetComponent<SpriteRenderer>();
        currentSpriteRenderer.sprite = BackgroundBase;
        currentSpriteRenderer.color = new Color(0, 0, 0, 0);
        currentSpriteRenderer.sortingLayerName = "Pieces";
        currentSpriteRenderer.sortingOrder = 9999;

        BoardManager.foregroundUI.transform.position = new Vector3(BoardManager.offset.x, BoardManager.offset.y, 0);
        float bgScale = BoardManager.horizontalUnitSize / 8f;
        BoardManager.foregroundUI.transform.localScale = new Vector3(bgScale, bgScale, 0);
    }
}
