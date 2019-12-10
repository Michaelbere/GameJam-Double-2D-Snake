using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private bool _flashing = false;

    void OnEnable()
    {
        EventManager.ColorChanges += FlashColors;
        EventManager.FlipProcedure += StopFlashing;
    }

    private void StopFlashing()
    {
        _flashing = false;
    }

    private void OnDestroy()
    {
        EventManager.ColorChanges -= FlashColors;
        EventManager.FlipProcedure -= StopFlashing;   
    }

    private void FlashColors()
    {
        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        Debug.Log("Called FlashColors");
        //MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Color original = meshRenderers[0].material.color; // Assumes all materials are the same!
        Color newColor = GetNewColor(original);
        float flashTime = 0.5f; // Half a second between color changes
        _flashing = true;
        // Flash all children
        foreach (var meshRenderer in meshRenderers)
        {
            StartCoroutine(Flash(meshRenderer, original, newColor, flashTime));
        }
    }

    IEnumerator Flash(MeshRenderer meshRenderer, Color original, Color newColor, float flashTime)
    {
        // Set the new color to flash to
        Color flashColor = newColor;
        while (_flashing)
        {
            Debug.Log("changed");
            meshRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashTime);
            //Change the color to change to between flashes
            flashColor = flashColor == original ? newColor : original;
        }
        // Change the color back to the original
        meshRenderer.material.color = original;
    }

    /// <summary>
    /// Returns a new Color to create the flashing effect
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    private Color GetNewColor(Color original)
    {
        Color newColor = original;
//        newColor.a -= 0.5f; // Reduce alpha for some sort of flash effect
        //TODO: this can be changed to whatever the designers want later. This a placeholder
        newColor = Color.red;
        return newColor;
    }
}