using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceScript : MonoBehaviour
{
    public int index = 0;
    public bool isHappy;

    public Vector2Int gridPosition;

    public int playerSpriteIndex = 0;
    public Sprite[] sadPlayerSprites;
    public Sprite[] happyPlayerSprites;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = gameObject.transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateTexture();
    }

    bool inTransition;
    float maxTime;
    float timer;
    Vector3 originalPos;
    Vector3 deltaPos;

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0 && inTransition)
        {
            float temp = Func((maxTime - timer) / maxTime);
            //Debug.Log(temp);
            gameObject.transform.position = originalPos + deltaPos * temp;
        }
        else if (inTransition)
        {
            //Debug.Log("out of transition");
            gameObject.transform.position = originalPos + deltaPos;
            originalPos = gameObject.transform.position;
            inTransition = false;
        }
        timer -= Time.deltaTime;
    }

    public void RaisePower(int amount)
    {
        index += amount;
        UpdateTexture();
    }
    public void UpdateTexture()
    {
        Sprite newSprite = null;
        string newName = "";

        if (index >= 1 && index < 20)
        {
            newSprite = GlobalData.powSpriteArray[index - 1];
            newName = string.Format("Piece 2^{0}, Pos:{1};{2}", index, gridPosition.x, gridPosition.y);
        }
        else if (index == 20)
        {
            if (!isHappy)
                newSprite = sadPlayerSprites[playerSpriteIndex];
            else
                newSprite = happyPlayerSprites[playerSpriteIndex];

        }

        //Assignment
        if (newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
            spriteRenderer.sortingOrder = index;
        }
        if (newName != "")
            name = newName;
    }
    public void MoveTo(Vector2Int newGridPos, Vector3 position, float time)
    {
        inTransition = true;
        deltaPos = position - originalPos;
        maxTime = time;
        timer = time;
        gridPosition = newGridPos;
    }

    public float Func(float x)
    {
        if (x < 1)
            return Mathf.Sqrt(x);
        else if (x < 0)
            return 0;
        return 1;
    }
}
