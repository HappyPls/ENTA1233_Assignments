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
    [SerializeField] private float LifeTime = 5f;
    [SerializeField] private BaseBulletManager shooterManager;

    private bool hasHit = false;

    public void Initialize(BaseBulletManager manager)
    {
        shooterManager = manager;
    }
    public void Start()
    {
        Rb.AddForce(transform.forward * ProjectileSpeed, ForceMode.Impulse);
        Destroy(gameObject, LifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        float knockbackForce = 10f;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            enemy.OnDamage(25, hitDirection, knockbackForce);
            hasHit = true;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            hasHit = true;
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            player.TakeDamage((int)ProjectileDamage, hitDirection, knockbackForce);
            Destroy(gameObject);
            return;
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (!hasHit)
        {
            Debug.Log("Bullet Missed!");
        }
    }
}
