using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    private Coroutine coroutine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(coroutine != null) StopCoroutine(coroutine);

            coroutine = StartCoroutine(GameManager.Instance.Player.controller.SpeedBoost());
            //Destroy(gameObject);
        }
    }


    

}
