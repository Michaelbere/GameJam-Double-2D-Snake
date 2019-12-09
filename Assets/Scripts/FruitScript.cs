using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public Transform plane;

    public BoardTileGenerator board;

    protected int minX, minZ, maxX, maxZ;
    protected float radius;

    public float expireTime = 15f;
    protected float expiryCounter = 0f;
    // Start is called before the first frame update
    protected void Start()
    {
        var scale = plane.localScale;
        minX = (int)(-scale.x / 2);
        minZ = (int)(-scale.z / 2);
        maxX = (int)(scale.x / 2);
        maxZ = (int)(scale.z / 2);
        radius = gameObject.GetComponent<SphereCollider>().radius + 0.05f;  // add 0.05 to keep spawn numbers round
    }

    // Update is called once per frame
    void Update()
    {
        expiryCounter += Time.deltaTime;
        if (expiryCounter > expireTime)
        {
            expiryCounter = 0;
            getNewLocation(1);
        }
    }

    public void getNewLocation(int zCoordinate)
    {
        transform.localPosition = board.getFreePosition(zCoordinate);
    } 
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Head"))
        {
//            Debug.Log("Eated");
            expiryCounter = 0;
            getNewLocation(1);
        }
    }
}
