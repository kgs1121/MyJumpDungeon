using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !GameManager.Instance.Player.controller.isOnLadder)
        {
            GameManager.Instance.Player.controller.SetLadder(true);
            GameManager.Instance.Player.controller.rb.velocity = Vector3.zero;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && GameManager.Instance.Player.controller.isOnLadder)
        {
            GameManager.Instance.Player.controller.SetLadder(false);
        }
    }


}
