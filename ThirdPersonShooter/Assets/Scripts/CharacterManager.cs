using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public int health = 100;
    public bool isAiming = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null )
        {
            Debug.LogWarning("Animator not found on player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        AimingManager();
        Debug.Log("Player Position: " + transform.position);
    }

    void AimingManager()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = !isAiming;

            if (animator != null)
            {
                animator.SetBool("Aiming", isAiming);
            }

            Debug.Log("Aiming: " + isAiming);
        }
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player took damage. Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player has died.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.Log("GameManager instance not found!");
        }
    }
}
