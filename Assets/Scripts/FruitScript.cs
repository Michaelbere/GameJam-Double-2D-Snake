﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public BoardTileGenerator board;

    public float expireTime = 15f;
    protected float expiryCounter = 0f;
    // Start is called before the first frame update
    protected void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        expiryCounter += Time.deltaTime;
        if (expiryCounter > expireTime)
        {
            expiryCounter = 0;
            getNewLocation(1);
        }
    }

    public void getNewLocation(int zCoordinate)
    {
        transform.localPosition = board.getFreePosition(zCoordinate);
    } 
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Head"))
        {
//            Debug.Log("Eated");
            expiryCounter = 0;
            getNewLocation(1);
        }
    }
}
