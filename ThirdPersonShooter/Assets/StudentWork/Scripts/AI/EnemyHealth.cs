using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("Visual")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    private Color originalColor;

    [Header("Effects")]
    [SerializeField] private GameObject deathEffect;

    private void Start()
    {
        currentHP = maxHP;

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }
    
    public void OnDamage(int amount, Vector3 hitDirection, float knockbackForce = 5f)
    {
        currentHP -= amount;
        FlashHit();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null )
        {
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode.Impulse);
        }    
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void FlashHit()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = hitColor;
            Invoke(nameof(ResetColor), flashDuration);
        }
    }

    private void ResetColor()
    {
       if(enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
