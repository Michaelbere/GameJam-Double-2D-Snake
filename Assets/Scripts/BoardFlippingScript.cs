using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardFlippingScript : MonoBehaviour
{
    public Transform snakeHead;
    public Material water;
    public Material desert;
    // For bomb instantiation
    public GameObject bomb;
    public Transform darkPlane;

    public float newBombTime = 15f;
    protected float newBombCounter = 0f;
    // end For bomb instantiation
    private bool sunnySideUp = true;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = desert;
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 1.5f);
        if (!isSunnySideUp())
        {
            newBombCounter += Time.deltaTime;
        }
        if (newBombCounter > newBombTime)
        {
            newBombCounter = 0;
            newBombTime += 5;
            newBomb();
        }
    }


    public void flip(SnakeHeadScript.Flip flip, float duration)
    {
        sunnySideUp = !sunnySideUp;
        switch (flip)
        {
            case SnakeHeadScript.Flip.LEFT:
                transform.DOLocalRotate(Vector3.back * 180, duration, mode: RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.RIGHT:
                transform.DOLocalRotate(Vector3.forward * 180, duration, mode: RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.UP:
                transform.DOLocalRotate(Vector3.left * 180, duration, mode: RotateMode.LocalAxisAdd);
                break;
            case SnakeHeadScript.Flip.DOWN:
                transform.DOLocalRotate(Vector3.right * 180, duration, mode: RotateMode.LocalAxisAdd);
                break;
        }
        if (RenderSettings.skybox == water) RenderSettings.skybox = desert;
        else RenderSettings.skybox = water;
    }

    public bool isSunnySideUp()
    {
        return sunnySideUp;
    }

    private void newBomb()
    {
        Debug.Log("create new bomb");
        /* TODO finish initialization of bomb -> the starting location vector is not relevant only it's y value is 
        important to set because it doesnt change and this is the way to set which plane the pickup is on
        (-1) for dark plane, (1) for sunny*/
        //        GameObject newPart = Instantiate(bomb, new Vector3(-0.5f, 1, -8.5f), Quaternion.identity) ;
        //        newPart.transform.SetParent(transform);
        //        var s = newPart.GetComponent<BombScript>();
        //        newPart.GetComponent<BombScript>().plane = darkPlane;
        //        s.gameBoard = gameObject;
    }
}
