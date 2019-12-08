using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private BarController _dryBar;
    private BarController _airBar;

    private bool _upSide = false;

    // Start is called before the first frame update
    void Start()
    {
        _dryBar = GameObject.Find("DryBar").GetComponent<BarController>();
        _airBar = GameObject.Find("AirBar").GetComponent<BarController>();
//        _airBar.setBar(0);
    }

    // Update is called once per frame
    void Update()
    {
        _dryBar.UpdateBar(_upSide);
        _airBar.UpdateBar(!_upSide);
    }
    /// <summary>
    /// Used to activate the Manager
    /// </summary>
    public void Activate()
    {
    }
    /// <summary>
    /// Handles flipping the board
    /// </summary>
    public void Flip()
    {
        _upSide = !_upSide;
    }
}