using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePowButtonScr : MonoBehaviour
{

    public GameObject editorCanvas;
    EditorData editorData;

    public Image underlay;
    public Image overlay;

    public int selectID;
    public Color unselectedColor;
    public Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        underlay.color = unselectedColor;
        editorCanvas = GameObject.Find("EditorCanvas");
        editorData = editorCanvas.GetComponent<EditorData>();
        overlay.sprite = GlobalData.powSpriteArray[selectID - 1];

        EditorEventScript.current.onSelectedChange += WhenSelectedChanged;
    }
    public void DoWhenPressed()
    {
        if (editorData.currentSelectedIndex != selectID)
            editorData.currentSelectedIndex = selectID;
        else
            editorData.selectedButton = "";

        EditorEventScript.current.SelectedChange();

        //Exit aka invoke the function the darken overlay would do
        GameObject.Find("PopUpOverlay").GetComponent<Button>().onClick.Invoke();
    }

    public void WhenSelectedChanged()
    {
        if (selectID == editorData.currentSelectedIndex)
            underlay.color = selectedColor;
        else
            underlay.color = unselectedColor;
    }
}
