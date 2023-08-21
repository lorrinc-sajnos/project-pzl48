using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pieceSprites : MonoBehaviour
{
    //Textures
    public Texture2D twoPowBase;
    public Vector2Int twoPowSize;

    public int spriteCount;
    public Sprite[] twoPowSprites;

    // Start is called before the first frame update
    void Start()
    {
        twoPowSprites = new Sprite[twoPowSize.x * twoPowSize.y];
        int counter = 0;
        for (int i = twoPowSize.y - 1; i >= 0; i--)
        {
            for (int j = 0; j < twoPowSize.x; j++)
            {
                twoPowSprites[counter] = Sprite.Create(twoPowBase, new Rect(256 * j, 256 * i, 256, 256), new Vector2(), 128);
                counter++;
            }
        }
    }
}
