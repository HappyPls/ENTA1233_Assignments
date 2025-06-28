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
    [SerializeField] private Image GameOver;

    private void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int amount, Vector3 hitDirection, float knockbackForce = 10f)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        UpdateUI();

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

    private void UpdateUI()
    {
        if (HP != null)
        {
            HP.fillAmount = (float)currentHP / maxHP;
        }
    }
}
