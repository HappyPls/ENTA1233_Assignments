using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleSystem;

    [SerializeField] private float LifeTime = 5f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ParticleSystem.IsAlive() == false)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
        if (timer > LifeTime)
            Destroy(gameObject);
    }
}
