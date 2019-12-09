﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int _score = 0;

//    private enum State
//    {
//        Flipping,
//        Running
//    }
    private bool _flipping = false;

    //Controllers for the different bars
    private BarController _dryBar;

    private BarController _airBar;

    // Current state of the game 
//    private State _gameState = State.Running;

    //Boolean telling bars if they are "active" or not
    private bool _upSide = false;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: move this to another place if manager is needed in main menu for score
        SetControllers();
    }

//    private void OnEnable()
//    {
//        EventManager.FlipProcedure += Flip;
//    }

    // Update is called once per frame
    void Update()
    {
//        if (_gameState == State.Running)
        if (!_flipping)
        {
            UpdateBars();
        }
    }


    /// <summary>
    /// Used to activate the Manager
    /// </summary>
    public void Activate()
    {
    }
    // ================================ Score Handling ================================ //

    public void UpdateScore(int newScore)
    {
        _score = newScore;
    }


    // ================================ Bar Controlling ================================ //


    private void SetControllers()
    {
        _dryBar = GameObject.Find("DryBar").GetComponent<BarController>();
        _airBar = GameObject.Find("AirBar").GetComponent<BarController>();
    }

    private void UpdateBars()
    {
        _dryBar.UpdateBar(_upSide);
        _airBar.UpdateBar(!_upSide);
    }

    /// <summary>
    /// Handles flipping the board
    /// </summary>
    public void Flip()
    {
//        _gameState = State.Flipping;
        _flipping = true;
        _upSide = !_upSide;
    }

    public void ContinueRunning()
    {
        _flipping = false;
    }
}