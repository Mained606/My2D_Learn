using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class Attack : MonoBehaviour
    {
        //공격력
        public float attackDamage = 10f;

        //충돌 체크해서 공격력 만큼 데미지 준다
        public void OnTriggerEnter2D(Collider2D collision)
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            if(damageable != null)
            {
                Debug.Log(collision.name);
                damageable.TakeDamage(attackDamage);
            }
        }
    }

}
