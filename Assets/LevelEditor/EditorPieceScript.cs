using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class EditorPieceScript : MonoBehaviour
{
    public EditorData editorData;

    public Image underlay;
    public Image goalImage;
    public Image overlay;

    const int playerIndex = 20;
    const int emptyIndex = 255;

    public int index;
    public bool isTwoPow;
    public bool hasGoal;

    //Underlay Sprites
    public Color originalColor;
    public Sprite playerSprite;


    public void VarInit()
    {
        originalColor = underlay.color;
        overlay.enabled = false;
        //editorData = GameObject.Find("EditorCanvas").GetComponent<EditorData>();
    }

    public void UpdateMyself()
    {
        switch (editorData.selectedButton)
        {
            case "twoPow":
                {
                    if (index != emptyIndex)
                    {
                        isTwoPow = true;
                        index = editorData.currentSelectedIndex;
                    }
                    break;
                }
            case "player":
                {
                    if (index != emptyIndex)
                    {
                        isTwoPow = false;
                        index = playerIndex;
                    }

                    break;
                }
            case "goal":
                {
                    if (index != emptyIndex)
                    {
                        if (!hasGoal)
                            hasGoal = true;
                        else
                            hasGoal = false;
                    }
                    break;
                }
            case "delete":
                {
                    if (index == 0)
                    {
                        index = emptyIndex;
                        isTwoPow = false;
                        hasGoal = false;
                    }
                    else
                        index = 0;
                    break;
                }
        }
        UpdateTexture();
    }

    public void UpdateTexture()
    {
        //Underlay
        if (index == emptyIndex)
        {
            goalImage.enabled = false;
            underlay.color = new Color(1, 1, 1, 0);
        }
        else if (hasGoal)
        {
            goalImage.enabled = true;
            underlay.color = new Color(1, 1, 1, 0);
        }
        else
        {
            goalImage.enabled = false;
            underlay.color = originalColor;
        }

        //Overlay
        overlay.enabled = true;

        if (index == 0 || index == emptyIndex)
            overlay.enabled = false;

        else if (index == playerIndex)
            overlay.sprite = playerSprite;

        else if (isTwoPow)
            overlay.sprite = GlobalData.powSpriteArray[index - 1];

    }
}
