using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public GameObject plane;

    private int minX, minZ, maxX, maxZ;
    private float radius;

    private float expireTime = 5f;
    private float expiryTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        var scale = plane.transform.localScale;
        minX = (int)(-scale.x / 2);
        minZ = (int)(-scale.z / 2);
        maxX = (int)(scale.x / 2);
        maxZ = (int)(scale.z / 2);
        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        expiryTimer += Time.deltaTime;
        if (expiryTimer > expireTime)
        {
            expiryTimer = 0;
            getNewLocation();
        }
    }

    private void getNewLocation(Collider target=null)
    {
        float newX = Random.Range(minX, maxX) + radius;
        float newZ = Random.Range(minZ, maxZ) + radius;
        transform.localPosition = new Vector3(newX, transform.localPosition.y , newZ);
    } 
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Head"))
        {
            getNewLocation(target);
            Debug.Log("Eated");
        }
    }
}
