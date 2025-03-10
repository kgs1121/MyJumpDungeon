using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Moveplatform : MonoBehaviour
{
    public Vector3 startPosition;

    public float speed = 1f;

    public float distance = 4f;

    public float platformRotate;

    private void Start()
    {
        startPosition = transform.position;
        
    }
    private void Update()
    {
        platformRotate = startPosition.x + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);
        transform.position = new Vector3(platformRotate, startPosition.y, startPosition.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(transform);
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.SetParent(null);
    }

}
