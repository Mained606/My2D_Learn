using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class Projectile : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;

        //이동
        [SerializeField] private Vector2 moveSpeed = new Vector2(5f, 0f);


        //데미지
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private Vector2 knockback = new Vector2(0f, 0f);

        //데미지 이펙트
        public GameObject ImpactEffectPrefab;

        #endregion

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb2D.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
        }

        //히트

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damageable damageable = collision.GetComponent<Damageable>();
            //knockback 방향 설정
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage, deliveredKnockback);

                //데미지 이펙트
                GameObject effectGo = Instantiate(ImpactEffectPrefab, collision.transform.position, Quaternion.identity);
                Destroy(effectGo, 0.5f);

                //화살 킬
                Destroy(this.gameObject);
            }

        }
    }

}
