using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyScript : MonoBehaviour
{
    public Transform prevBodyPart;
    public Transform nextBodyPart;


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
            if (didEat)
            {
                GameObject newPart = Instantiate(Resources.Load("Body")) as GameObject;
                newPart.transform.SetParent(transform.parent);
                SnakeBodyScript newPartScript = newPart.GetComponent<SnakeBodyScript>();
                newPartScript.prevBodyPart = transform;
                GetComponent<SnakeBodyScript>().nextBodyPart = newPart.transform;
                nextBodyPart.GetComponent<SnakeBodyScript>().move(oldPosition, false);
                Vector3 relativePos = newPosition - oldPosition;
                transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            }
        }
        else
        {
            Vector3 relativePos = newPosition - oldPosition;
            transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            nextBodyPart.GetComponent<SnakeBodyScript>().move(oldPosition, didEat);
        }
    }

    
}