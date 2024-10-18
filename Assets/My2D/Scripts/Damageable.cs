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
        private UIManager uiManager;

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
            uiManager = FindObjectOfType<UIManager>();

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

                //데미지 전의 hp
                float beforeHealth = CurrentHealth;
                
                //체력 감소
                CurrentHealth -= damage;
                Debug.Log(transform.name + " : " + CurrentHealth);

                //애니메이션 트리거 실행
                animator.SetTrigger(AnimationString.HitTrigger);

                //데미지 효과
                hitAction?.Invoke(damage, knockback);


                float realDamage = beforeHealth - CurrentHealth;


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

            //힐 전의 hp
            float beforeHealth = CurrentHealth;
            
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

            //실제 힐 hp 값
            float realHealth = CurrentHealth - beforeHealth;

            ////2024-10-17 추가
            //// 받은 힐링값이 최대 체력 - 현재 체력보다 크면 최대 체력 - 현재 체력을 매개변수로 보내줌
            //if(MaxHealth - CurrentHealth < amount)
            //{
            //    uiManager.GetCurrentHealth(MaxHealth - CurrentHealth);
            //}
            //// 받은 힐링값이 최대 체력 - 현재 체력보다 작으면 받은 힐링값을 매개변수로 보내줌
            //else
            //{
            //    uiManager.GetCurrentHealth(amount);
            //}


            //체력 회복 이벤트 발생
            CharacterEvents.characterHealed?.Invoke(gameObject, realHealth);
            
            return true;
        }

    }

}
