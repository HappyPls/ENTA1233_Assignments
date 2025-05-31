using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBullet : MonoBehaviour
{

    [SerializeField] private float ProjectileSpeed = 100f;
    [SerializeField] private float ProjectileDamage;
    [SerializeField] private Rigidbody Rb;
    [SerializeField] private float LifeTime = 10f;

    public void Start()
    {
        Rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
        Destroy(gameObject, LifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        //ContactPoint contact = collision.GetCollision();
        //BulletManager.OnProjectileCollision();
        Destroy(gameObject);
    }
}
