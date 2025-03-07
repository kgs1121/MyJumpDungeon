using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public static float plusSpeed = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.condition.SpeedUp(plusSpeed);
            //Destroy(gameObject);
        }
    }
}
