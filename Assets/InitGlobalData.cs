using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGlobalData : MonoBehaviour
{
    public Sprite[] powSpriteArray;

    // Start is called before the first frame update
    public void Init()
    {
        GlobalData.powSpriteArray = (Sprite[])powSpriteArray.Clone();

        GlobalData.CustomLevelsInit();
    }
}
