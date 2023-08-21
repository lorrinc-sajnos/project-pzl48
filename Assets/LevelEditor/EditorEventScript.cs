using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorEventScript : MonoBehaviour
{
    public static EditorEventScript current;

    void Awake()
    {
        current = this;
    }

    public event Action onSelectedChange;
    public event Action onSelectedPowChange;
    public void SelectedChange()
    {
        if (onSelectedChange != null)
        {
            onSelectedChange();
        }
    }
    public void SelectedPowChange()
    {
        if (onSelectedPowChange != null)
        {
            onSelectedPowChange();
        }
    }
}
