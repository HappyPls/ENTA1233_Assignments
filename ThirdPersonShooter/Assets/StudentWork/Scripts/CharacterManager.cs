using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;

    public int health = 100;
    public bool isAiming = false;

    public void SpawnCharacter()
    {
        Vector3 spawnPosition = Vector3.zero;
        Instantiate(characterPrefab, spawnPosition, Quaternion.identity, transform);
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
