using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    public GameObject plane;

    private int minX, minZ, maxX, maxZ;
    private float radius;

    public float expireTime = 15f;
    private float expiryCounter = 0f;
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
        expiryCounter += Time.deltaTime;
        if (expiryCounter > expireTime)
        {
            expiryCounter = 0;
            getNewLocation();
        }
    }

    private void getNewLocation(Collider target=null)
    {
        Collider[] checkResult;
        float newX;
        float newZ;
        do
        {
            newX = Random.Range(minX, maxX) + radius;
            newZ = Random.Range(minZ, maxZ) + radius;
            checkResult = Physics.OverlapSphere(new Vector3(newX, transform.localPosition.y, newZ), 0.49f);
        } while (checkResult.Length != 0);
        
        transform.localPosition = new Vector3(newX, transform.localPosition.y , newZ);
    } 
    
    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.CompareTag("Head"))
        {
            Debug.Log("Eated");
            expiryCounter = 0;
            getNewLocation(target);
        }
    }
}
