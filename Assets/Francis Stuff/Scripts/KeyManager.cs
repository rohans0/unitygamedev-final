using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    // Simple script for one key and one door
   public bool hasKey = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerObject")){
            hasKey = true;
            Destroy(gameObject);
        }
    }
}
