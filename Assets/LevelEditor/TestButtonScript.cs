using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButtonScript : MonoBehaviour
{
    public EditorData editorData;

    public void DoWhenPressed()
    {
        GlobalData.isLevelEditorMode = true;
        GlobalData.levelTag = "editor";
        editorData.moves = new List<Vector2Int>();

        GlobalData.levelToLoad = editorData.GenerateLevelData(0);
        GlobalData.LOADLevelToLoad();
    }
}
