using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : PickUps
{
    [SerializeField] private int HealthRecovered = 20;
    void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.RecoverHP(20);
            Destroy(gameObject);
        }
    }
}
