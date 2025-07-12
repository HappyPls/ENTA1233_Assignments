using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgUp : PickUps
{
    [SerializeField] private float bonusDamage = 10f;
    protected override void ApplyStats(PlayerStats stats)
    {
        stats.AddBonusDamage(10f);
    }
}
