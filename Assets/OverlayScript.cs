using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayScript : MonoBehaviour
{
    public GameObject savePopUp;

    public void StartDisable()
    {
        savePopUp.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void StartEnable()
    {
        savePopUp.gameObject.SetActive(true);
        savePopUp.gameObject.SetActive(true);
    }
}
