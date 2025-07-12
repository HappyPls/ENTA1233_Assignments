using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgUp : PickUps
{
    [SerializeField] private float bonusDamage = 10f;
    void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null )
        {
            stats.AddBonusDamage(10f);
            Destroy(gameObject);
        }
    }
}
