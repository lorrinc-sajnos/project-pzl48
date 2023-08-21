using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LinkKeypressToButton : MonoBehaviour
{
    public KeyCode key;

    Button thisButton;

    void Start()
    {
        thisButton = gameObject.GetComponent<Button>();
    }
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            thisButton.onClick.Invoke();
        }
    }
}
