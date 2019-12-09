using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // ======================== Color Changing ======================== //

    public delegate void ColorChange();

    public static event ColorChange ColorChanges;

    public static void ChangeColors()
    {
        ColorChanges?.Invoke();
    }


    // ======================== Flipping ======================== //
    public delegate void FlipFunction();

    public static event FlipFunction FlipProcedure;

    /// <summary>
    /// Called when flipping the board
    /// </summary>
    public static void EnterFlip()
    {
        FlipProcedure?.Invoke();
    }

    public delegate void FlipExit();

    public static event FlipExit FlipExitProcedure;

    /// <summary>
    /// Called when done flipping the board
    /// </summary>
    public static void StopFlip()
    {
        FlipExitProcedure?.Invoke();
    }

    // ======================== Game Resetting ======================== //

    public delegate void ResetFunc();

    public static event ResetFunc ResetProcedure;

    public static void ResetGame()
    {
        ResetProcedure?.Invoke();
    }
}