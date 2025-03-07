using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().hasLost = true;
        }
    }
}
