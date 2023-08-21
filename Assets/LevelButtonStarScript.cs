using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonStarScript : MonoBehaviour
{
    public Color emptyColor;
    public Color gotColor;

    [SerializeField]
    public Image[] stars;

    // Start is called before the first frame update
    public void AssignStars(int starCount)
    {
        for (int i=0;i<3;i++)
        {
            if (i < starCount)
                stars[i].color = gotColor;
            else
                stars[i].color = emptyColor;
        }
    }

}
