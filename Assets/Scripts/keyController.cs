using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyController : MonoBehaviour
{
    public GameObject door_bottom;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(door_bottom);
            Destroy(gameObject);
        }
    }
}
