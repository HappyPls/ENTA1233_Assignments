using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    protected int maxAmmo = 30;
    protected int damage = 10;
    protected float fireRate = 1.0f;
    protected float reloadTime 5.0f;

    protected int currentAmmo;
    protected bool isReloading = false;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
    }
}
