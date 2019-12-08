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
    public static void DoFlip()
    {
        FlipProcedure?.Invoke();
    }
    
    

}
