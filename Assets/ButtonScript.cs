using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public string levelName;
    public int index;
    public Color buttonColor;

    public Text buttonText;
    public Color buttonTextColor;

    public Image image; 

    // Start is called before the first frame update
    void Start()
    {
        levelName = index.ToString();
        buttonText.text = levelName;
        buttonText.color = buttonTextColor;
        image.color = buttonColor;
    }

    public void LoadLevel()
    {
        GlobalData.LoadFromIndex(index);
    }
}
