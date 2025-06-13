using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BulletManager : BaseBulletManager
    {
        [Header("External Scripts")]
        [SerializeField] private Camera Cam;
        [SerializeField] private PlayerInputActions PlayerInputs;

        [Header("Raycast")]
        [SerializeField] private LayerMask RaycastMask;
        [SerializeField] private ShootType ShootingCalculation;

            public enum ShootType
        {
            Raycast = 0,
            Physics = 1,
        }

        private void Update()
        {
            if (PlayerInputs.ToggleFire)
            {
                ToggleShootType();
                PlayerInputs.ToggleFire = false; // reset after toggle
            }

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
                    SpawnPhysicsBullet(Cam.transform);                    
                    break;
                default:
                    Debug.LogError("Unexpected Value");
                    break;

            }
        }


        private void DoRaycastShot()
        {
            Debug.Log("Raycasting");

            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, Mathf.Infinity, RaycastMask))
            {
                Debug.Log("Raycast Hit!");

                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    Debug.Log("Enemy hit! Applying Damage.");
                    Vector3 hitDirection = hit.collider.transform.position - Cam.transform.position;
                    enemy.OnDamage(25, hitDirection);
                }

                OnProjectileCollision(hit.point, hit.normal);
            }
            else
            {
                Debug.Log("Raycast Miss!");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if(PlayerInputs.Aim)
            Gizmos.DrawLine(Cam.transform.position, Cam.transform.position + Cam.transform.forward * 100);
        }
        private void ToggleShootType()
        {
            ShootingCalculation = ShootingCalculation == ShootType.Raycast ? ShootType.Physics : ShootType.Raycast;
            Debug.Log("Switched Fire Mode to: " + ShootingCalculation);
        }

        private void CleanupParticle()
        {

        }
    }
}
