using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectPieceScript : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public EditorData editorData;
    public Image overlay;

    public float timeToHold;
    public UnityEvent whenHolded;
    bool isHolding = false;
    float timer=0;

    // Start is called before the first frame update
    void Start()
    {
        EditorEventScript.current.onSelectedChange += WhenSelectedChanged;
    }

    void Update()
    {
        if (isHolding)
            timer += Time.deltaTime;

        if (timer >= timeToHold)
        {
            editorData.selectedButton = "twoPow";
            EditorEventScript.current.SelectedChange();
            whenHolded.Invoke();
        }
    }

    public void WhenSelectedChanged()
    {
        overlay.sprite = GlobalData.powSpriteArray[editorData.currentSelectedIndex - 1];
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        timer = 0;
    }

}
