using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    private float floatSpeed = 1f;
    private float floatHeight = 0.15f;

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
}
