using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevelSlcButton : MonoBehaviour
{
    public int index;

    public Text levelName;
    public Text authorName;
    public LevelButtonStarScript stars;


    // Start is called before the first frame update
    public void Generate(int Index)
    {
        index = Index;
        levelName.text = GlobalData.customLvls[index].levelName;
        authorName.text = "by: " + GlobalData.customLvls[index].author;
        stars.AssignStars(GlobalData.customLvls[index].starsAchived);
    }
}
