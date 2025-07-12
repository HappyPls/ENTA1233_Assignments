using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private float chargeTime = 0.7f;
    [SerializeField] private float projectileSpeed = 50f;
    [SerializeField] private float projectileDamage = 25f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private GameObject explosionEffect;

    private BaseBulletManager shooterManager;
    private Action onFireComplete;
    private bool hasHit = false;
    private GameObject lastHitObject;

    public void Initialize(BaseBulletManager manager, Action onComplete)
    {
        shooterManager = manager;
        onFireComplete = onComplete;

        if (rb == null) rb = GetComponent<Rigidbody>();
        PlayerStats stats = manager.GetComponent<PlayerStats>();
        if (stats != null )
        {
            projectileDamage = stats.GetTotalDamage();
        }
        StartCoroutine(ChargeAndLaunch());
    }
    private IEnumerator ChargeAndLaunch()
    {
        rb.isKinematic = true;
        Vector3 initialScale = Vector3.one * 0.1f;
        Vector3 finalScale = Vector3.one * 0.5f;
        transform.localScale = initialScale;

        float elapsed = 0f;

        while (elapsed < chargeTime)
        {
            float t = elapsed / chargeTime;
            transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = finalScale;

        yield return new WaitForSeconds(chargeTime);

        rb.isKinematic = false;
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);

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
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.TakeDamage((int)projectileDamage, hitDirection, knockbackForce);
        }

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (!hasHit || (lastHitObject != null && lastHitObject.layer == LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Fireball fizzled.");
        }
    }
}