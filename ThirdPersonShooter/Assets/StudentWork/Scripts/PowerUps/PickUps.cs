using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUps : MonoBehaviour
{
    private float floatSpeed = 1.5f;
    private float floatHeight = 0.20f;

    private float rotationSpeed = 100f;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("PickUp Update running");

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        transform.Rotate(Vector3.up, rotationSpeed *Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats != null)
        {
            ApplyStats(stats);
            Destroy(gameObject);
        }
    }
    protected abstract void ApplyStats(PlayerStats stats);
}
