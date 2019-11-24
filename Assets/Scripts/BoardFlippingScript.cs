using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardFlippingScript : MonoBehaviour
{
    public Transform snakeHead;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void flip(SnakeHeadScript.Flip flip, float duration)
    {
        switch (flip)
        {
            case SnakeHeadScript.Flip.LEFT:
                transform.DOLocalRotate(Vector3.back * 180, duration, mode:RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.RIGHT:
                transform.DOLocalRotate(Vector3.forward * 180, duration, mode: RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.UP:
                transform.DOLocalRotate(Vector3.left * 180, duration, mode:RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.DOWN:
                transform.DOLocalRotate(Vector3.right * 180, duration, mode:RotateMode.LocalAxisAdd);
                break;
        }
    }
}
