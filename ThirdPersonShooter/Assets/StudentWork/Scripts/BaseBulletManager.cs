using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class BaseBulletManager : MonoBehaviour
{
    [Header("Bullets")]
    [SerializeField] private PhysicsBullet PhysicsBulletPrefab;

    [Header("Particle")]
    [SerializeField] private RaycastBullet BulletParticle;

    protected void SpawnPhysicsBullet(Transform shooterTransform)
    {
        PhysicsBullet spawnedBullet = Instantiate(PhysicsBulletPrefab, shooterTransform.transform.position, shooterTransform.transform.rotation);
        spawnedBullet.Initialize(this);
    }

    protected void OnProjectileCollision(Vector3 position, Vector3 rotation)
    {
        SpawnParticle(position, rotation);
    }

    private void SpawnParticle(Vector3 position, Vector3 rotation)
    {
        Instantiate(BulletParticle, position, Quaternion.Euler(rotation));
    }
}
