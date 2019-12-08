using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int _score = 0;

    //Controllers for the different bars
    private BarController _dryBar;

    private BarController _airBar;

    //Boolean telling bars if they are "active" or not
    private bool _upSide = false;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: move this to another place if manager is needed in main menu for score
        SetControllers();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();
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
        _upSide = !_upSide;
    }
    
    
    
}