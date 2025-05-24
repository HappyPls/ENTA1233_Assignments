using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    [Header("Gun Settings")]
    protected int maxAmmo = 30;
    protected int damage = 10;
    protected float fireRate = 1.0f;
    protected float reloadTime = 5.0f;

    //can create a reference to get animation, vfx and audio.

    protected int currentAmmo;
    protected bool isReloading = false;

    protected virtual void Start()
    {
        currentAmmo = maxAmmo;
    }

    protected virtual void Update()
    {
        //check if the gun is reloading
        //if reload input key is pressed, start the reload animation and pause any other firing actions.
        //change currentAmmo to maxAmmo on animation end
        //Debug.Log ("Gun Reloaded!");
        // if firing input is pressed, stop reload animation and keep currentAmmo instead of changing it to maxAmmo
        return;
    }

    protected virtual void Shoot()
    {
        //check if currentAmmo = 0. If ammo is 0, start reload animation and disable shooting
        //shooting script here
        return;
    }
}
