using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBullet : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 50f;
    [SerializeField] private float projectileDamage = 25f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifeTime = 5f;

    private BaseBulletManager shooterManager;
    private Action onFireComplete;
    private bool hasHit = false;
    private GameObject lastHitObject;

    public void Initialize(BaseBulletManager manager, Action onComplete)
    {
        shooterManager = manager;
        onFireComplete = onComplete;

        if (rb == null) rb = GetComponent<Rigidbody>();

        Fire();
    }
    private void Fire()
    {
        rb.isKinematic = false;
        rb.velocity = transform.forward * projectileSpeed;
        onFireComplete?.Invoke();
        Destroy(gameObject, lifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        hasHit = true;
        lastHitObject = other.gameObject;

        Vector3 hitDirection = other.transform.position - transform.position;

        //Enemy Damage
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.OnDamage((int)projectileDamage, hitDirection, transform, knockbackForce);
        }

        //Player Damage
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage((int)projectileDamage, hitDirection, knockbackForce);
        }

        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (!hasHit || (lastHitObject != null && lastHitObject.layer == LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Bullet Missed");
        }
    }
}
