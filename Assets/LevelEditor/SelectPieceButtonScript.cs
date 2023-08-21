using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPieceButtonScript : MonoBehaviour
{
    public GameObject editorCanvas;
    EditorData editorData;

    public Image underlay;
    public Image overlay;

    public string selectID;
    public Color unselectedColor;
    public Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        underlay.color = unselectedColor;
        editorCanvas = GameObject.Find("EditorCanvas");
        editorData = editorCanvas.GetComponent<EditorData>();
        WhenSelectedChanged();
        EditorEventScript.current.onSelectedChange += WhenSelectedChanged;
    }
    public void DoWhenPressed()
    {
        if (editorData.selectedButton != selectID)
            editorData.selectedButton = selectID;
        else
            editorData.selectedButton = "";

        EditorEventScript.current.SelectedChange();
    }

    public void WhenSelectedChanged()
    {
        if (selectID == editorData.selectedButton)
            underlay.color = selectedColor;
        else
            underlay.color = unselectedColor;
    }
}
