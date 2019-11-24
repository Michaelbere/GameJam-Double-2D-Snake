using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadScript : SnakeBodyScript
{

    [Range(0.1f, 2)]
    public float snakeSpeed = 0.5f;
    public Transform board;
    private float snakeMoveTimer;

    private bool didEat = false;
    // Start is called before the first frame update
    enum PlayerDirection
    {
        UP, DOWN, RIGHT, LEFT
    }

    public enum Flip
    {
        RIGHT, LEFT, UP, DOWN, NO_FLIP
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

    IEnumerator DelayedMove(Vector3 newPosition, float delay, bool isLast)
    {
        // Debug.Log("DelayedMove called with " + newPosition.ToString());
        yield return new WaitForSeconds(delay);
        // Debug.Log("DelayedMove ended with " + newPosition.ToString());
        move(newPosition, false);
        if (isLast)
        {
            flip = Flip.NO_FLIP;
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
                answer[3] = answer[2] + (answer[2].x < 0 ?
                Vector3.right : Vector3.left);
                break;
            case Flip.UP:
            case Flip.DOWN:
                answer[3] = answer[2] + (answer[2].z < 0 ?
                Vector3.forward : Vector3.back);
                break;
        }
        answer[3] = answer[2];
        return answer;
    }

    void Start()
    {
        snakeMoveTimer = snakeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        snakeMoveTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Space)){
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
        if (snakeMoveTimer < 0 && flip == Flip.NO_FLIP)
        {
            snakeMoveTimer += snakeSpeed; // check that this doesnt fail in any way
                                          //(the idea is that if it is too low the
                                          //time passed will go on to the next cycle
                                          // to keep cycle length consistant)
            Vector3 newPosition;
            switch (direction)
            {
                case PlayerDirection.UP:
                    newPosition = transform.localPosition + Vector3.forward * verticalMultiplier;
                    break;
                case PlayerDirection.DOWN:
                    newPosition = transform.localPosition + Vector3.back * verticalMultiplier;
                    break;
                case PlayerDirection.RIGHT:
                    newPosition = transform.localPosition + Vector3.right * horizontalMultiplier;
                    break;
                case PlayerDirection.LEFT:
                    newPosition = transform.localPosition + Vector3.left * horizontalMultiplier;
                    break;
                default:
                    newPosition = transform.localPosition;
                    break;
            }
            flip = calculateFlip(newPosition);
            if (flip != Flip.NO_FLIP)
            {
                Debug.Log(flip);
                board.GetComponent<BoardFlippingScript>().flip(flip, snakeSpeed * 3);
                Vector3[] positionSteps = calculatePositionSteps(newPosition, flip);
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
                StartCoroutine(DelayedMove(positionSteps[0], snakeSpeed * 0, false));
                StartCoroutine(DelayedMove(positionSteps[1], snakeSpeed * 1, false));
                StartCoroutine(DelayedMove(positionSteps[2], snakeSpeed * 2, false));
                StartCoroutine(DelayedMove(positionSteps[3], snakeSpeed * 3, true));
            }
            else
            {
                // TODO add method to check if eaten and send instead of false
                move(newPosition,didEat);
                didEat = false;
            }
        }
    }
}
