using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class Damageable : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        //체력
        [SerializeField] private float maxHealth = 100f;
        public float MaxHealth{
            get{return maxHealth;}
            set{maxHealth = value;}
        }

        private float currentHealth;
        public float CurrentHealth{
            get{return currentHealth;}
            set
            {
                currentHealth = value;
                if(currentHealth <= 0)
                {
                    IsDead = true;
                    
                }
            }
        }

        private bool isDead = false;
        public bool IsDead{
            get{return isDead;}
            private set
            {
                isDead = value;
                animator.SetBool(AnimationString.IsDead, value);
            }
        }

        private bool isInvincible = false;
        [SerializeField] private float invincibleTimer = 3f;
        private float countdown = 0f;
        #endregion
        //TakeDamage
    

        void Awake()
        {
            animator = GetComponent<Animator>();

        }
        
        void Start()
        {
            CurrentHealth = MaxHealth;
            countdown = invincibleTimer;
        }

        void Update()
        {
            if(isInvincible)
            {
                if(countdown <= 0)
                {
                    isInvincible = false;
                    countdown = invincibleTimer;
                }
                countdown -= Time.deltaTime;
                
            }
        }
        public void TakeDamage(float damage)
        {
            if(!IsDead && !isInvincible)
            {
                isInvincible = true;
                
                //체력 감소
                CurrentHealth -= damage;
                Debug.Log(transform.name + " : " + CurrentHealth);

                //애니메이션 트리거 실행
                animator.SetTrigger(AnimationString.HitTrigger);
            }
        }

    }

}
