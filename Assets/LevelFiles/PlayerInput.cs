using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public GameplayLogic gameplayLogic;

    public bool keyboardEnabled = true;
    public Vector2 touchStart = new Vector2(-1, -1);
    public float swipeDist;
    public float degreeTolerance;

    public float inputDelay;
    float inputTimer;
    public bool hasSwiped = false;


    Vector2 fingPos;
    void Update()
    {
        //Look for input if timer is up
        if (inputTimer <= 0)
        //if (false)
        {
            //Keyboard controlls
            if (keyboardEnabled)
            {
                //Up
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    CallMove(Vector2Int.up);
                }
                //Down
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    CallMove(Vector2Int.down);
                }
                //Left
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    CallMove(Vector2Int.left);
                }
                //Right
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    CallMove(Vector2Int.right);
                }

                //Restart
                if (Input.GetKeyDown(KeyCode.R))
                {
                    GlobalData.levelToLoad.FromFile("lvl1-1.lvl");
                    GlobalData.LOADLevelToLoad();
                }
            }

            //Touch controlls
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    Debug.Log("One finger started touching");
                    hasSwiped = false;
                    touchStart = Input.touches[0].position;
                }
                fingPos = Input.touches[0].position;
                if (Vector2.Distance(touchStart, fingPos) >= swipeDist && !hasSwiped)
                {
                    Debug.Log("Swiped!");

                    hasSwiped = true;

                    //Calcularte angle
                    Debug.Log("Calculate angle");
                    float angle = Vector2.SignedAngle(fingPos - touchStart, Vector2.left);
                    //Evaluate swipe
                    Debug.Log("Calculate direction");
                    Vector2Int dir = EvaluateDirection(angle);
                    Debug.Log("Call direction");
                    CallMove(dir);
                }
            }
        }
        else
            inputTimer -= Time.deltaTime;
    }

    public void CallMove(Vector2Int dir)
    {
        inputTimer = inputDelay;
        if (dir != Vector2Int.zero)
            gameplayLogic.Move(dir);
    }
    public Vector2Int EvaluateDirection(float angle)
    {
        Debug.Log("Swipe complete with angle : " + angle);
        float degTolHalf = degreeTolerance / 2;

        //check up
        if (angle >= 90 - degTolHalf && angle <= 90 + degTolHalf)
            return Vector2Int.up;
        //check left
        if (angle >= 180 - degTolHalf || angle <= -180 + degTolHalf)
            return Vector2Int.right;
        //check down
        if (angle >= -90 - degTolHalf && angle <= -90 + degTolHalf)
            return Vector2Int.down;
        //check right
        if (angle >= 0 - degTolHalf && angle <= 0 + degTolHalf)
            return Vector2Int.left;

        return
            Vector2Int.zero;
    }

}
