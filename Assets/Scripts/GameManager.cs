using System;
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
}