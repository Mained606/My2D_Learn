using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    public class Damageable : MonoBehaviour
    {
        #region Variables
        private Animator animator;

        //델리게이트
        //등록된 함수를 호출하는 이벤트
        public UnityAction<float, Vector2> hitAction;

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

        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.LockVelocity);
            }
            private set
            {
                animator.SetBool(AnimationString.LockVelocity, value);
            }
        }
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
        public void TakeDamage(float damage, Vector2 knockback)
        {
            if(!IsDead && !isInvincible)
            {
                isInvincible = true;
                
                //체력 감소
                CurrentHealth -= damage;
                Debug.Log(transform.name + " : " + CurrentHealth);

                //입은 데미지 텍스트 전달


                //애니메이션 트리거 실행
                animator.SetTrigger(AnimationString.HitTrigger);

                //데미지 효과
                hitAction?.Invoke(damage, knockback);
                CharacterEvents.characterDamaged?.Invoke(gameObject, damage);
            }
        }

        //체력 회복
        public bool Heal(float amount)
        {
            
            if(CurrentHealth >= MaxHealth)
            {
                return false;
            }
            
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);


            //체력 회복 이벤트 발생
            CharacterEvents.characterHealed?.Invoke(gameObject, amount);
            
            return true;
        }

    }

}
