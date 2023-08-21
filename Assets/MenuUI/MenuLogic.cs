using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuLogic : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;
    public GameObject customLevelSelect;
    public CustomLvlSelectionScript customLvlSelButtons;
    public InitGlobalData initGlobalData;

    public InputField levelCode;

    public Color bgColor;

    GlobalData globalDataLogic;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(GlobalData.menuTag == "main");
        levelSelect.SetActive(GlobalData.menuTag == "lvlSelect");
        customLevelSelect.SetActive(GlobalData.menuTag == "customLvlSelect");

        Camera.main.backgroundColor = bgColor;

        initGlobalData.Init();
        customLvlSelButtons.GenerateButtons();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        //TEST
        if (Input.GetKeyDown(KeyCode.R))
        {
            globalDataLogic.levelToLoad.FromFile("lvl1-1.lvl");
            globalDataLogic.LOADLevelToLoad();
        }
    }
    */

    public void LoadEditor()
    {
        GlobalData.LoadEditor();
    }

    public void PlayFromCode()
    {
        GlobalData.levelTag = "customLvl";

        GlobalData.levelToLoad.FromBase64(levelCode.text);
        GlobalData.LOADLevelToLoad();
    }

    public void SaveFromCode()
    {
        LevelData level;
        try { level = LevelData.CreateFromBase64(levelCode.text); }
        catch (LevelData.LevelIOException) { return; }

        string lvlPath = string.Format("{0}/{1}_by_{2}.{3}", LevelInfo.customLvlDir, level.levelName, level.authorName, LevelInfo.extension);
        Debug.Log(lvlPath);
        LevelInfo save = LevelInfo.FromLevelData(lvlPath, level, (ushort)GlobalData.customLvls.Length);

        save.SaveToBinaryFile();

        customLvlSelButtons.GenerateButtons();

        GlobalData.CustomLevelsInit();
    }
}
