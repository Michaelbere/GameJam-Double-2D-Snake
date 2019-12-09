using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : FruitScript
{
    public GameObject gameBoard;

    void Start()
    {
        base.Start();
        expireTime = 25f;
    }
    void Update()
    {
        bool countTime = !gameBoard.GetComponent<BoardFlippingScript>().isSunnySideUp();
        expiryCounter += countTime ? Time.deltaTime : 0;
//        if (countTime){Debug.Log("counting");}
        if (expiryCounter > expireTime)
        {
            expiryCounter = 0;
            getNewLocation();
        }
    }
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Head"))
        {
            Debug.Log("Bombed");  // this is just so the bomb wont jump when touched
        }
    }
}
