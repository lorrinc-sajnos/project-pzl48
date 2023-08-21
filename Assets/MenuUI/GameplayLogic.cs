using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLogic : MonoBehaviour
{
    public boardManager BoardManager;
    public movePieces MovePieces;
    public LevelData levelData;
    public UIManagment uiManagement;

    public GlobalData globalDataLogic;


    public bool hasWon;

    public bool outOfMoves = false;

    //Gameplay variables

    public int moveCount = 0;

    //UI variables
    public float[] starPrc = { 1.8f, 1.5f, 1f };
    public int starCount = 3;

    // Start is called before the first frame update
    void Start()
    {
        levelData = GlobalData.levelToLoad;

        uiManagement.InitalizeUI(BoardManager.offset);
        BoardManager.StartLevel();
    }
    void Update()
    {
        if (isTransitioning)
        {
            //Debug.Log("color transition");
            foregfroundUISprite.color = Color.Lerp(startColor, endColor, 1 - timer / maxTimer);
            timer -= Time.deltaTime;
            if (timer <= 0)
                isTransitioning = false;
        }
    }


    public void Move(Vector2Int direction)
    {
        if (!hasWon)
        {
            if (MovePieces.CalculateMove(direction))
            {
                if (GlobalData.isLevelEditorMode)
                    GlobalData.moves.Add(direction);

                moveCount++;
                uiManagement.SetMoves(moveCount);
                //Debug.Log("Star count: " + StarFromMoves());
                if (starCount != StarFromMoves())
                {
                    starCount = StarFromMoves();
                    uiManagement.SetStars(starCount);
                }
            }
        }
    }

    public void DoWhenWin()
    {
        hasWon = true;
        BoardManager.enabled = false;
        Debug.Log("--YOU WON---");

        uiManagement.OverlayText(uiManagement.winText);
        ForegroundTransition(uiManagement.overlayTime, uiManagement.winOverlayColor, uiManagement.overlayAlpha);
    }

    public int StarFromMoves()
    {

        //3 stars
        if (levelData.solutionCount != 0)
        {
            if (moveCount <= levelData.movesForStars[2])
                return 3;
            else if (moveCount <= levelData.movesForStars[1])
                return 2;
            else if (moveCount <= levelData.movesForStars[0])
                return 1;
            else
                return 0;
        }
        else
            return 3;
    }

    public void BackToMenu()
    {
        /*
        if (globalDataLogic.isLevelEditorMode)
        {
            globalDataLogic.LoadEditor();
            return;
        }*/
        switch (GlobalData.levelTag)
        {
            case "customLvl":
                GlobalData.BackToMenu("customLvlSelect");
                return;

            case "editor":
                GlobalData.LoadEditor();
                return;
            default:
                GlobalData.BackToMenu("lvlSelect");
                return;
        }
    }

    //Foreground sprites
    Color startColor;
    Color endColor;
    public int endAlpha;
    bool isTransitioning = false;
    public SpriteRenderer foregfroundUISprite;
    float timer;
    float maxTimer;
    public void ForegroundTransition(float time, Color color, float maxAplha)
    {
        startColor = new Color(color.r, color.g, color.b, 0);
        endColor = new Color(color.r, color.g, color.b, maxAplha);
        //Debug.Log(startColor + " " + endColor);
        timer = time;
        maxTimer = time;
        foregfroundUISprite = BoardManager.foregroundUI.GetComponent<SpriteRenderer>();
        isTransitioning = true;
    }
}
