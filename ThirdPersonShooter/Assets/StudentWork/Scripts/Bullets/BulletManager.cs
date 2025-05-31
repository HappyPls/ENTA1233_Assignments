using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private float bulletForce = 100f;
        [SerializeField] private Camera Cam;
        //[SerializeField] private GameObject Player;
        //[SerializeField] private GameObject Barrel;

        [Header("Bullets")]
        [SerializeField] private PhysicsBullet PhysicsBulletPrefab;
        [SerializeField] private RaycastBullet BulletParticle;

        [SerializeField] private PlayerInputActions PlayerInputs;

        [SerializeField] private LayerMask RaycastMask;

        [SerializeField] private ShootType ShootingCalculation;

            public enum ShootType
        {
            Raycast = 0,
            Physics = 1,
        }

        private void Update()
        {
            if (PlayerInputs.Aim && PlayerInputs.Fire) OnFirePressed();
            PlayerInputs.Fire = false;
        }

        private void OnFirePressed()
        {
            Debug.Log("Shooting Projectile!");
            switch (ShootingCalculation)
            {
                case ShootType.Raycast:
                    DoRaycastShot();                    
                    break;
                case ShootType.Physics:
                    SpawnPhysicsBullet();                    
                    break;
                default:
                    Debug.LogError("Unexpected Value");
                    break;

            }
        }
        private void SpawnPhysicsBullet()
        {
            PhysicsBullet spawnedBullet =Instantiate(PhysicsBulletPrefab, Cam.transform.position, Cam.transform.rotation);
            //spawnedBullet = (this);
        }

        private void DoRaycastShot()
        {
            Debug.Log("Raycasting");
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, Mathf.Infinity, RaycastMask))

            {
                Debug.Log("Raycast Hit!");
                OnProjectileCollision(hit.point, hit.normal);
            }
            else
            {
                Debug.Log("Raycast Miss!");
            }
        }

        private void OnProjectileCollision(Vector3 position, Vector3 rotation)
        {
            SpawnParticle(position, rotation);
        }

        private void SpawnParticle(Vector3 position, Vector3 rotation)
        {
            Instantiate(BulletParticle, position, Quaternion.Euler(rotation));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(PlayerInputs.Aim)
            Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward * 100);
        }

        private void CleanupParticle()
        {

        }
    }
}
