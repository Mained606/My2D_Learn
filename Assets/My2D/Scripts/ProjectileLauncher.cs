using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class ProjectileLauncher : MonoBehaviour
    {
        #region Variables
        public GameObject projectilePrefab;
        public Transform firePoint;
        #endregion

        public void FireProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectilePrefab.transform.rotation);
            Destroy(projectile, 5f);

            // 화살의 방향 결정
            Vector3 originScale = projectile.transform.localScale;
            projectile.transform.localScale = new Vector3(

                originScale.x * transform.localScale.x > 0? 1: -1,
                originScale.y,
                originScale.z

             );

        }
    }

}
