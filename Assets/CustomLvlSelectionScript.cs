using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLvlSelectionScript : MonoBehaviour
{
    public GameObject levelSelectButton;
    public GameObject parent;

    GameObject[] buttons;

    public void GenerateButtons()
    {
        DeleteButtons();
        int buttonCount = GlobalData.customLvls.Length;
        buttons = new GameObject[buttonCount];

        for (int i = 0; i < buttonCount; i++)
        {
            buttons[i] = Instantiate(levelSelectButton, parent.transform);
            var temp = buttons[i].GetComponent<CustomLevelSlcButton>();
            temp.Generate(i);

        }
    }
    void DeleteButtons()
    {
        if (buttons != null)
            foreach (var v in buttons)
                Destroy(v);
    }
}
