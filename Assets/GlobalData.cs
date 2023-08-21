using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using System.IO;

public class GlobalData
{
    //This is the gloal data, and it is shared across The menus and levels

    public static string levelTag;
    public static LevelData levelToLoad = new LevelData();

    public static LevelInfo[] levels;
    public static int lastVisitedLevel;

    public static LevelInfo[] customLvls;

    public static string menuTag = "main";

    public static Sprite[] powSpriteArray;




    //Level Editor fields
    public static bool isLevelEditorMode;
    public static List<Vector2Int> moves = new List<Vector2Int>();

    public static void LOADLevelToLoad()
    {
        SceneManager.LoadScene("levelScene");
    }

    public static void BackToMenu(string tag)
    {
        isLevelEditorMode = false;
        menuTag = tag;
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadEditor()
    {
        SceneManager.LoadScene("Leveleditor");
    }

    public static void LoadFromIndex(int lvlIndex)
    {
        levelToLoad = levels[lvlIndex].levelData;
        LOADLevelToLoad();
    }

    public static void CustomLevelsInit()
    {

        if (Directory.Exists(LevelInfo.customLvlDir))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + LevelInfo.customLvlDir, "*." + LevelInfo.extension);
            Debug.Log("Files found in : " + LevelInfo.customLvlDir + " c:" + files.Length);

            customLvls = new LevelInfo[files.Length];

            for (int i=0;i<files.Length;i++)
            {
                string[] temp = files[i].Split('/');
                customLvls[i] = LevelInfo.CreateFromBinaryFile("/" + temp[temp.Length - 1]);
            }


        }
        else
        {
            Directory.CreateDirectory(LevelInfo.customLvlDir);
            customLvls = new LevelInfo[0];
        }

    }
}
