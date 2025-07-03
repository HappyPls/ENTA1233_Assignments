using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Shown Stats")]
    [Tooltip("Set player information")]
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [Header("PlayerUI")]
    [Tooltip("Set Player UI")]
    [SerializeField] private Image HP;
    [SerializeField] private Image DelayedHP;
    [SerializeField] private Image GameOver;

    [Header("Delayed Effect Settings")]
    [SerializeField] private float delaySpeed = 0.5f;

    private float targetFillAmount;

    private void Start()
    {
        currentHP = maxHP;
        targetFillAmount = 1f;
        UpdateUI(true);
    }

    public void TakeDamage(int amount, Vector3 hitDirection, float knockbackForce = 10f)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        targetFillAmount = (float)currentHP / maxHP;
        UpdateUI(false);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode.Impulse);
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        if (GameOver != null)
        {
            GameOver.gameObject.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (DelayedHP != null)
        {
            if (DelayedHP.fillAmount > targetFillAmount)
            {
                DelayedHP.fillAmount -= delaySpeed * Time.deltaTime;
                DelayedHP.fillAmount = Mathf.Max(DelayedHP.fillAmount, targetFillAmount);
            }
        }
    }

    private void UpdateUI(bool instantUpdate)
    {
        
        if (HP != null)
        {
            float fill = (float)currentHP / maxHP;
            HP.fillAmount = fill;

            if (instantUpdate && DelayedHP != null)
            {
                DelayedHP.fillAmount = fill;
            }
        }
    }
}
