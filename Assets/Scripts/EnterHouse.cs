using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHouse : MonoBehaviour
{
    public GameObject Enemy;
    public DayNightCycle DayNightCycle;
    public Transform housePos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Enemy.SetActive(true);
            DayNightCycle.timeOfDay = 6.0f;
            other.transform.position = housePos.position;
        }
    }
}
