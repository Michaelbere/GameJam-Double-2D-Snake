using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = System.Numerics.Quaternion;
using TMPro;
using UnityEngine.SceneManagement;

public class SnakeHeadScript : SnakeBodyScript
{
    [Range(0.1f, 2)] public float snakeSpeed = 0.5f;
    public Transform board;
    public TextMeshProUGUI GameoverText;
    public BoardTileGenerator boardTileGenerator;
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

    public enum FlipType
    {
        WALL,
        HOLE
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
    private PlayerDirection newDirection = PlayerDirection.UP;
    private PlayerDirection realDirection = PlayerDirection.UP;

    private int verticalMultiplier = 1;
    private int horizontalMultiplier = 1;

    private void Death()
    {
        GameoverText.gameObject.SetActive(true);
        GameoverText.text = "You Scored: " + score;

        StartCoroutine(GameOverDelay());
    }


    (Flip, FlipType) calculateFlip(Vector3 newPosition)
    {
        if (newPosition.x < 0)
        {
            return (newPosition.y > 0 ? Flip.LEFT : Flip.RIGHT, FlipType.WALL);
        }

        if (newPosition.x >= boardTileGenerator.getWidth())
        {
            return (newPosition.y < 0 ? Flip.LEFT : Flip.RIGHT, FlipType.WALL);
        }

        if (newPosition.z < 0)
        {
            return (newPosition.y > 0 ? Flip.DOWN : Flip.UP, FlipType.WALL);
        }

        if (newPosition.z >= boardTileGenerator.getHeight())
        {
            return (newPosition.y < 0 ? Flip.DOWN : Flip.UP, FlipType.WALL);
        }

        if (boardTileGenerator.isHole(newPosition))
        {
            return (
                (realDirection == PlayerDirection.UP || realDirection == PlayerDirection.DOWN) ? Flip.RIGHT : Flip.DOWN,
                FlipType.HOLE);
        }

        return (Flip.NO_FLIP, FlipType.WALL);
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

    IEnumerator GameOverDelay()
    {
        Time.timeScale = .0000001f;
        yield return new WaitForSeconds(3 * Time.timeScale);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Scenes/Menu");
    }

    Vector3[] calculatePositionSteps(Vector3 newPosition, Flip flip, FlipType flipType)
    {
        if (flipType == FlipType.WALL)
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

            return answer;
        }
        else
        {
            Vector3[] answer = new Vector3[6];
            answer[0] = newPosition;
            answer[1] = Vector3.Scale(newPosition, Vector3.forward + Vector3.right);
            switch (flip)
            {
                case Flip.LEFT:
                case Flip.RIGHT:
                    answer[2] = answer[1] + verticalMultiplier *
                                (realDirection == PlayerDirection.UP ? Vector3.forward : Vector3.back);
                    answer[3] = answer[2] + verticalMultiplier *
                                (realDirection == PlayerDirection.UP ? Vector3.forward : Vector3.back);
                    answer[4] = answer[3] + Vector3.Scale(newPosition, Vector3.down);
                    answer[5] = answer[4] + verticalMultiplier *
                                (realDirection == PlayerDirection.UP ? Vector3.forward : Vector3.back);
                    break;
                case Flip.UP:
                case Flip.DOWN:
                    answer[2] = answer[1] + horizontalMultiplier *
                                (realDirection == PlayerDirection.RIGHT ? Vector3.right : Vector3.left);
                    answer[3] = answer[2] + horizontalMultiplier *
                                (realDirection == PlayerDirection.RIGHT ? Vector3.right : Vector3.left);
                    answer[4] = answer[3] + Vector3.Scale(newPosition, Vector3.down);
                    answer[5] = answer[4] + horizontalMultiplier *
                                (realDirection == PlayerDirection.RIGHT ? Vector3.right : Vector3.left);

                    // answer[5] = answer[4] + horizontalMultiplier * (realDirection == PlayerDirection.RIGHT ? Vector3.right : Vector3.left);
                    break;
            }

            return answer;
        }
    }


    void Start()
    {
        snakeMoveTimer = snakeSpeed;
        EventManager.ResetProcedure += Death;
        setScoreText();
    }

    private void OnDestroy()
    {
        EventManager.ResetProcedure -= Death;
    }

    // Update is called once per frame
    void Update()
    {
        snakeMoveTimer -= Time.deltaTime;
        HandleInput();
        if (snakeMoveTimer < 0)
        {
            realDirection = newDirection;
            snakeMoveTimer += snakeSpeed; // check that this doesnt fail in any way
            //(the idea is that if it is too low the
            //time passed will go on to the next cycle
            // to keep cycle length consistant)
            if (flip == Flip.NO_FLIP)
            {
                Vector3 newPosition = GetNewPosition();
                FlipType flipType;
                (flip, flipType) = calculateFlip(newPosition);
                if (flip != Flip.NO_FLIP)
                {
                    //This should be called instead of everything else
                    //EventManager.DoFlip();

                    //TODO: Delegate this
                    EventManager.EnterFlip();
                    // Erez here, would like gamemanager to change to flipping state when a flip starts
                    board.GetComponent<BoardFlippingScript>().flip(flip, snakeSpeed * 3);
                    Vector3[] positionSteps = calculatePositionSteps(newPosition, flip, flipType);
                    ChangeMovementDirection();
                    StartCoroutine(DelayedMove(positionSteps, snakeSpeed));
                    StartCoroutine(DelayedResetFlip(snakeSpeed * (positionSteps.Length - 1)));
                    // Erez here, would like gamemanager to change back to running state after a flip is finished
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
            GameManager.Instance.IncrementScore();
            didEat = true;
            setScoreText();
        }

        if (target.gameObject.CompareTag("Body") || target.gameObject.CompareTag("Bomb"))
        {
//            Debug.Log("Dead");
//            Time.timeScale = 0f;
            EventManager.ResetGame();
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
        switch (realDirection)
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
        if (Input.GetKey(KeyCode.LeftArrow) && realDirection != PlayerDirection.RIGHT)
        {
            newDirection = PlayerDirection.LEFT;
        }

        if (Input.GetKey(KeyCode.RightArrow) && realDirection != PlayerDirection.LEFT)
        {
            newDirection = PlayerDirection.RIGHT;
        }

        if (Input.GetKey(KeyCode.DownArrow) && realDirection != PlayerDirection.UP)
        {
            newDirection = PlayerDirection.DOWN;
        }

        if (Input.GetKey(KeyCode.UpArrow) && realDirection != PlayerDirection.DOWN)
        {
            newDirection = PlayerDirection.UP;
        }
    }
}