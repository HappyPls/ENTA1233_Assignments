using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class BaseBulletManager : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private FireballProjectile PhysicsBulletPrefab;

    [Header("Particle")]
    [SerializeField] private RaycastBullet BulletParticle;

    

    protected void SpawnPhysicsBullet(Transform firePoint, Vector3 direction)
    {
        if (PhysicsBulletPrefab == null)
        {
            Debug.LogWarning("PhysicsBulletPrefab not assigned!");
            return;
        }

        FireballProjectile spawnedBullet = Instantiate(
            PhysicsBulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        spawnedBullet.Initialize(this, null);
    }

    protected void OnProjectileCollision(Vector3 position, Vector3 rotation)
    {
        SpawnParticle(position, rotation);
    }

    private void SpawnParticle(Vector3 position, Vector3 rotation)
    {
        if (BulletParticle != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(rotation);
            Instantiate(BulletParticle, position, lookRotation);
        }
    }

}
