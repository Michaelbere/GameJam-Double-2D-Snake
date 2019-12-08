﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyScript : MonoBehaviour
{

    public Transform prevBodyPart;
    public Transform nextBodyPart;
    
    private bool _flashing = false;
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    protected bool isTail()
    {
        return nextBodyPart == null;
    }

    public void move(Vector3 newPosition, bool didEat)
    {
        Vector3 oldPosition = transform.localPosition;
        transform.localPosition = newPosition;
        if (isTail())
        {
            if(didEat){
                GameObject newPart = Instantiate(Resources.Load("Body")) as GameObject;
                newPart.transform.SetParent(transform.parent);
                SnakeBodyScript newPartScript = newPart.GetComponent<SnakeBodyScript>();
                newPartScript.prevBodyPart = transform;
                GetComponent<SnakeBodyScript>().nextBodyPart = newPart.transform;
                nextBodyPart.GetComponent<SnakeBodyScript>().move(oldPosition, false);
            }
        }
        else
        {
            nextBodyPart.GetComponent<SnakeBodyScript>().move(oldPosition, didEat);
        }
    }

    IEnumerator Flash(Color original, Color newColor, float flashTime)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        Color flashColor = newColor;
        while (_flashing)
        {
            meshRenderer.material.color = newColor;
            yield return new WaitForSeconds(flashTime);
            flashColor = (flashColor == original) ? newColor : original;
        }
        meshRenderer.material.color = original;
    }
}
