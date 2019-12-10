using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryAirController : MonoBehaviour
{
    private BarController _dryBar;
    private BarController _airBar;
    private bool _flipping = false;
    private bool _upSide = false;


    // Start is called before the first frame update
    void Start()
    {
        SetControllers();
    }

    private void OnEnable()
    {
        EventManager.FlipProcedure += Flip;
        EventManager.FlipExitProcedure += ContinueRunning;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_flipping)
        {
            _dryBar.UpdateBar(_upSide);
            _airBar.UpdateBar(!_upSide); 
        }
    }


    private void SetControllers()
    {
        _dryBar = GameObject.Find("DryBar").GetComponent<BarController>();
        _airBar = GameObject.Find("AirBar").GetComponent<BarController>();
    }
    
    private void Flip()
    {
        _flipping = true;
        _upSide = !_upSide;
    }

    private void ContinueRunning()
    {
        _flipping = false;
    }
}