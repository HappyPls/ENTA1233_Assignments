using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : PickUps
{
    [SerializeField] private int HealthRecovered = 20;
    protected override void ApplyStats(PlayerStats stats)
    {
            stats.RecoverHP(20);
    }
}
