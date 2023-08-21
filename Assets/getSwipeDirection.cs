using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getSwipeDirection : MonoBehaviour
{
    public GameplayLogic gameplayLogic;

    //
    public float minDistToSwipe;
    public float maxTimeToSwipe;

    public float degreeTolerance;

    bool begunSwipe = false;
    float swipeTimer = 0;

    public Vector3 startPos;
    public Vector3 currentPos;

    public bool KeyboardSupport;

    void Start()
    {
        //MovePieces = BoardLogic.GetComponent<movePieces>();

        /*
        startSprite = new GameObject("Start Position");
        endSprite = new GameObject("End Position");

        startSprite.AddComponent<SpriteRenderer>();
        endSprite.AddComponent<SpriteRenderer>();

        SpriteRenderer mem = startSprite.GetComponent<SpriteRenderer>();
        mem.sprite = startTexture;
        mem = endSprite.GetComponent<SpriteRenderer>();
        mem.sprite = endTexture;*/
    }

    Vector2Int direction;
    // Update is called once per frame
    void Update()
    {
        //*
        if (KeyboardSupport)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Pressed Up");
                direction = Vector2Int.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Pressed Down");
                direction = Vector2Int.down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Pressed Left");
                direction = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Pressed Right");
                direction = Vector2Int.right;
            }
            
            if (direction != Vector2Int.zero)
            {
                direction = Vector2Int.zero;
                Debug.Log("Keypress Driection: " + direction);
                gameplayLogic.Move(direction);
            }

        }
        //*/

        if (begunSwipe && swipeTimer > 0)
        {
            swipeTimer -= Time.deltaTime;
        }

        if (Input.touchCount != 0)
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    {
                        Debug.Log("Start point set!");
                        startPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        if (!begunSwipe)
                        {
                            Debug.Log("Begun swipe!");
                            begunSwipe = true;
                            swipeTimer = maxTimeToSwipe;
                        }

                        currentPos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

                        if (swipeTimer > 0 && Vector3.Distance(startPos, currentPos) >= minDistToSwipe)
                        {
                            //startSprite.transform.position = startPos;
                            //endSprite.transform.position = currentPos;

                            direction = EvaluateDirection(Vector2.SignedAngle(currentPos - startPos, Vector2.left));

                            if (direction != Vector2Int.zero)
                            {
                                Debug.Log("Swipe Driection: " + direction);
                                gameplayLogic.Move(direction);
                            }
                            else
                                Debug.Log("Swipe is invalid");

                            Debug.Log("Swiped");
                            swipeTimer = 0;
                        }
                        break;
                    }
            }
        }
        else if (begunSwipe)
        {
            Debug.Log("Finger lifted");
            swipeTimer = 0;
            begunSwipe = false;
        }
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
