﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = System.Numerics.Quaternion;
using TMPro;

public class SnakeHeadScript : SnakeBodyScript
{
    [Range(0.1f, 2)] public float snakeSpeed = 0.5f;
    public Transform board;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private float snakeMoveTimer;

    private bool didEat = false;

    // Start is called before the first frame update
    enum PlayerDirection
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    public enum Flip
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        NO_FLIP
    }

    private Flip flip = Flip.NO_FLIP;
    private PlayerDirection direction = PlayerDirection.UP;

    private int verticalMultiplier = 1;
    private int horizontalMultiplier = 1;

    Flip calculateFlip(Vector3 newPosition)
    {
        if (newPosition.x < 0)
        {
            return newPosition.y > 0 ? Flip.LEFT : Flip.RIGHT;
        }
        else if (newPosition.x > 19)
        {
            return newPosition.y < 0 ? Flip.LEFT : Flip.RIGHT;
        }

        if (newPosition.z < 0)
        {
            return newPosition.y > 0 ? Flip.DOWN : Flip.UP;
        }
        else if (newPosition.z > 29)
        {
            return newPosition.y < 0 ? Flip.DOWN : Flip.UP;
        }

        return Flip.NO_FLIP;
    }

    IEnumerator DelayedResetFlip(float delay)
    {
        yield return new WaitForSeconds(delay);
        flip = Flip.NO_FLIP;
        EventManager.StopFlip();
    }

    IEnumerator DelayedMove(Vector3[] newPositions, float delay)
    {
        // Debug.Log("DelayedMove called with " + newPosition.ToString());
        // Debug.Log("DelayedMove ended with " + newPosition.ToString());
        for (int i = 0; i < newPositions.Length; ++i)
        {
            move(newPositions[i], false);
//            Debug.Log(newPositions[i]);
            yield return new WaitForSeconds(delay);
        }
    }

    Vector3[] calculatePositionSteps(Vector3 newPosition, Flip flip)
    {
        Vector3[] answer = new Vector3[4];
        answer[0] = newPosition;
        answer[1] = Vector3.Scale(newPosition, Vector3.forward + Vector3.right);
        answer[2] = Vector3.Scale(newPosition, Vector3.forward + Vector3.right + Vector3.down);
        switch (flip)
        {
            case Flip.LEFT:
            case Flip.RIGHT:
                answer[3] = answer[2] + (answer[2].x < 0 ? Vector3.right : Vector3.left);
                break;
            case Flip.UP:
            case Flip.DOWN:
                answer[3] = answer[2] + (answer[2].z < 0 ? Vector3.forward : Vector3.back);
                break;
        }

        // answer[3] = answer[2];
        return answer;
    }

    void Start()
    {
        snakeMoveTimer = snakeSpeed;
        setScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        snakeMoveTimer -= Time.deltaTime;
        HandleInput();
        if (snakeMoveTimer < 0)
        {
            snakeMoveTimer += snakeSpeed; // check that this doesnt fail in any way
            //(the idea is that if it is too low the
            //time passed will go on to the next cycle
            // to keep cycle length consistant)
            if (flip == Flip.NO_FLIP)
            {
                Vector3 newPosition = GetNewPosition();
                flip = calculateFlip(newPosition);
                if (flip != Flip.NO_FLIP)
                { 
                    Debug.Log(flip);
                    EventManager.EnterFlip();
                    board.GetComponent<BoardFlippingScript>().flip(flip, snakeSpeed * 3);
                    Vector3[] positionSteps = calculatePositionSteps(newPosition, flip);
                    ChangeMovementDirection();
                    StartCoroutine(DelayedMove(positionSteps, snakeSpeed));
                    StartCoroutine(DelayedResetFlip(snakeSpeed * (positionSteps.Length - 1)));
                }
                else
                {
                    // TODO add method to check if eaten and send instead of false - fixed: collision handles that
//                    Debug.Log("Move was called");
                    move(newPosition, didEat);
                    didEat = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Fruit"))
        {
            score += 1;
            didEat = true;
            setScoreText();
        }
        
        if (target.gameObject.CompareTag("Body") || target.gameObject.CompareTag("Bomb"))
        {
            Debug.Log("Dead");
            Time.timeScale = 0f;
        }
    }

    void setScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    
   private void ChangeMovementDirection()
    {
        switch (flip)
        {
            case Flip.LEFT:
            case Flip.RIGHT:
                horizontalMultiplier = horizontalMultiplier * -1;
                break;
            case Flip.UP:
            case Flip.DOWN:
                verticalMultiplier = verticalMultiplier * -1;
                break;
        }
    }


    private Vector3 GetNewPosition()
    {
        switch (direction)
        {
            case PlayerDirection.UP:
                return transform.localPosition + Vector3.forward * verticalMultiplier;
            case PlayerDirection.DOWN:
                return transform.localPosition + Vector3.back * verticalMultiplier;
            case PlayerDirection.RIGHT:
                return transform.localPosition + Vector3.right * horizontalMultiplier;
            case PlayerDirection.LEFT:
                return transform.localPosition + Vector3.left * horizontalMultiplier;
            default:
                return transform.localPosition;
        }
    }

    /// <summary>
    /// Handles user input
    /// </summary>
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            didEat = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && direction != PlayerDirection.RIGHT)
        {
            direction = PlayerDirection.LEFT;
        }

        if (Input.GetKey(KeyCode.RightArrow) && direction != PlayerDirection.LEFT)
        {
            direction = PlayerDirection.RIGHT;
        }

        if (Input.GetKey(KeyCode.DownArrow) && direction != PlayerDirection.UP)
        {
            direction = PlayerDirection.DOWN;
        }

        if (Input.GetKey(KeyCode.UpArrow) && direction != PlayerDirection.DOWN)
        {
            direction = PlayerDirection.UP;
        }
    }
}