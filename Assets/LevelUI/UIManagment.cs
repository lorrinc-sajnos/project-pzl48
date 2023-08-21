using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagment : MonoBehaviour
{
    //Level
    public GameObject gameLogic;
    LevelData levelData;

    //UI elements
    public Text levelName;
    public Text moves;
    public Text overlayText;
    public Image starsUI;

    public float overlayTime = 0.5f;

    public float overlayAlpha = 0.5f;
    public Color winOverlayColor;
    public Color loseOverlayColor;

    public int textAlpha = 1;
    public Color overlayTextColor;

    public string winText;
    public string loseText;

    public Color starColor;
    public Color emptyStarColor;
    public Image[] stars;

    Color startColor;
    Color endColor;

    void Update()
    {
        if (isTextOverlay)
        {
            overlayText.color = Color.Lerp(startColor, endColor, 1 - overlayTimer / overlayTime);
            overlayTimer -= Time.deltaTime;

            if (overlayTimer <= 0)
                isTextOverlay = false;
        }
    }
    //*/

    public void InitalizeUI(Vector2 overlayPos)
    {
        levelData = gameLogic.GetComponent<GameplayLogic>().levelData;

        levelName.text = levelData.levelName;
        overlayText.transform.position = Camera.main.WorldToScreenPoint(overlayPos);
        SetMoves(0);
        SetStars(3);
    }

    public void SetMoves(int moveCount)
    {
        moves.text = "Moves: " + moveCount;
    }

    public float[] starValues = { 1f, 0.75f, 0.5f, 0.266f };

    public void SetStars(int starCount)
    {
        switch(starCount)
        {
            case 0:
                stars[0].color = emptyStarColor;
                stars[1].color = emptyStarColor;
                stars[2].color = emptyStarColor;
                break;
            case 1:
                stars[0].color = starColor;
                stars[1].color = emptyStarColor;
                stars[2].color = emptyStarColor;
                break;
            case 2:
                stars[0].color = starColor;
                stars[1].color = starColor;
                stars[2].color = emptyStarColor;
                break;
                //3 stars aka default
            default:
                stars[0].color = starColor;
                stars[1].color = starColor;
                stars[2].color = starColor;
                break;
        }
    }


    bool isTextOverlay = false;
    float overlayTimer;

    public void OverlayText(string message)
    {
        overlayTimer = overlayTime;
        startColor = new Color(overlayTextColor.r, overlayTextColor.g, overlayTextColor.b, 0);
        endColor = new Color(overlayTextColor.r, overlayTextColor.g, overlayTextColor.b, textAlpha);
        overlayText.color = new Color(overlayTextColor.r, overlayTextColor.g, overlayTextColor.b, 0);

        overlayText.text = message;
        isTextOverlay = true;
    }



}
