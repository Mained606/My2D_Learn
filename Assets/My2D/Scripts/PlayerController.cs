using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace My2D
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private Animator animator;
        // 플레이어 바닥 확인
        TouchingDirections touchingDirections;
        private Damageable damageable;
        // private SpriteRenderer spriteRenderer;

        // 플레이어 걷기 속도
        public float walkSpeed = 4f;
        // 플레이어 뛰기 속도
        public float runSpeed = 8f;
        // 플레이어 공중 이동 속도
        public float airSpeed = 3f;
        // 플레이어 점프 파워
        public float jumpImpulse = 11f;

        //이동 여부
        public bool CanMove
        {
            get
            {
                return animator.GetBool(AnimationString.CanMove);
            }
        }

        // 플레이어 이동과 관련된 입력값
        private Vector2 moveInput;


        // 플레이어 상태에 따라 이동속도 변경
        public float CurrentMoveSpeed
        {
            get
            {
                if(CanMove)
                {
                    if (IsMoving && !touchingDirections.IsOnWall)
                    {
                        if(touchingDirections.IsGrounded)
                        {
                            return IsRunning ? runSpeed : walkSpeed;
                        }
                        else
                        {
                            return airSpeed;
                        }
                    }
                    return 0; // Idle 상태
                }
                else
                {
                    return 0; //움직이지 못할 때
                }
            }
        }

        //플레이어 이동 여부
        [SerializeField] private bool _isMoving = false;
        public bool IsMoving 
        {
            get 
            {
                return _isMoving;
            } 
            set 
            {
                _isMoving = value;
                animator.SetBool(AnimationString.IsMoving, value);
            }
        }

        // 플레이어 달리기 여부
        [SerializeField] private bool _isRunning = false;
        public bool IsRunning 
        {
            get 
            {
                return _isRunning;
            } 
            set 
            {
                _isRunning = value;
                animator.SetBool(AnimationString.IsRunning, value);
            }
        }

        // // 플레이어 점프 여부 - 실패
        // [SerializeField] private bool _isJumping = false;
        // public bool IsJumping 
        // {
        //     get
        //     {
        //         return _isJumping;
        //     }
        //     set
        //     {
        //         _isJumping = value;
        //         animator.SetBool(AnimationString.IsJumping, value);
        //     }
        // }

        // 플레이어 좌우 방향 결정
        [SerializeField] private bool _isFacingRight = true;
        public bool IsFacingRight 
        { 
            get
            {
                return _isFacingRight;
            }
            private set
            {
                if(_isFacingRight != value)
                {
                    transform.localScale *= new Vector2(-1, 1);
                    _isFacingRight = value;
                }
            }
        }

        public bool IsDead
        {
            get
            {
                return animator.GetBool(AnimationString.IsDead);
            }
        }

        #endregion

        void Awake()
        {
            // 참조
            rb2D = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();
            touchingDirections = this.GetComponent<TouchingDirections>();
            damageable = this.GetComponent<Damageable>();
            damageable.hitAction += OnHit; //데미지 입었을 때 호출되는 함수
            // spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        void FixedUpdate()
        {   
            if(!damageable.LockVelocity)
            {
                // 좌우 이동
                rb2D.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb2D.velocity.y);
            }
            animator.SetFloat(AnimationString.yVelocity, rb2D.velocity.y);
        }

        // 뉴 인풋 시스템 사용
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();

            if(IsDead)
            {
                IsMoving = false;
            }
            else
            {
                // 이동 여부 판단
                IsMoving = moveInput != Vector2.zero;
                SetPlayerFlip(moveInput);
            }
            // Debug.Log("context : " + moveInput);
        }

        // 플레이어 좌우 방향 결정
        public void SetPlayerFlip(Vector2 moveInput)
        {
            if(moveInput.x > 0 && !IsFacingRight)
            {
                IsFacingRight = true;
                // Debug.Log("오른쪽");
                // spriteRenderer.flipX = false;
            }
            else if(moveInput.x < 0 && IsFacingRight)
            {
                IsFacingRight = false;
                // Debug.Log("왼쪽");
                // spriteRenderer.flipX = true;
            }
        }

        // 달리기 구현
        public void OnRun(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                IsRunning = true;
            }
            else if(context.canceled)
            {
                IsRunning = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // 점프 입력 감지 && 바닥에 있을 때
            if(context.started && touchingDirections.IsGrounded)
            {
                // 점프 애니메이션 실행
                animator.SetTrigger(AnimationString.JumpTrigger);
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpImpulse);
            }
        }

        // 공격 구현
        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started && touchingDirections.IsGrounded)
            {
                // 공격 애니메이션 실행
                animator.SetTrigger(AnimationString.AttackTrigger);

                // 공격 로직

            }
        }

        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.velocity = new Vector2(knockback.x, rb2D.velocity.y + knockback.y);
        }

        // ======================================================================================


        // 공격 구현(실패) - 3
        // public void OnAttack(InputAction.CallbackContext context)
        // {
        //     if(context.started)
        //     {
        //         StartCoroutine(Attack());
        //     }
        // }

        // IEnumerator Attack()
        // {
        //     animator.SetTrigger(AnimationString.AttackTrigger);
        //     animator.SetBool("EndAttack", true);
        //     float comboTime = 2f;

        //     if(comboTime >= 0)
        //     {
        //         if(Input.GetMouseButtonDown(0))
        //         {
        //             animator.SetBool("EndAttack", false);
        //         }
        //         else
        //         {
        //             animator.SetBool("EndAttack", true);
        //         }
        //     }
        //     else
        //     {
        //         animator.SetBool("EndAttack", true);

        //     }
        //     comboTime -= Time.deltaTime;

        //     yield return null;
        // }

        // 공격 구현(실패) - 2
        // public void OnAttack(InputAction.CallbackContext context)
        // {
        //     if(context.started)
        //     {
        //         animator.SetTrigger(AnimationString.AttackTrigger);
        //         StartCoroutine(SecondAttack());
        //     }

        //     if(context.canceled)
        //     {
        //         animator.ResetTrigger(AnimationString.AttackTrigger);
        //     }
        // }

        // IEnumerator SecondAttack()
        // {
        //     if(Input.GetMouseButtonDown(0))
        //     {
        //         animator.SetBool("EndAttack", false);
        //         yield return new WaitForSeconds(0.5f);
        //         animator.SetBool("EndAttack", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("EndAttack", true);
        //         yield return null;
        //     }
        // }



        // 공격 구현(실패) - 1
        // public void OnAttack(InputAction.CallbackContext context)
        // {
        //     if(context.started)
        //     {
        //         animator.SetTrigger(AnimationString.AttackTrigger);

        //         if(context.started)
        //         {
        //             animator.SetTrigger("STrigger");

        //             if(context.started)
        //             {
        //                 animator.SetTrigger("TTrigger");
        //                 animator.ResetTrigger("TTrigger");
        //                 animator.SetBool("AttackCheck", false);
        //             }
        //             else
        //             {
        //                 animator.SetBool("AttackCheck", false);
        //                 animator.ResetTrigger("STrigger");
        //             }
        //         }
        //         else
        //         {
        //             animator.SetBool("AttackCheck", false);
        //             animator.ResetTrigger(AnimationString.AttackTrigger);
        //         }
        //     }
 
        // }
        
        // // 점프 구현 - (무한 점프 수정 필요)
        // public void OnJump(InputAction.CallbackContext context)
        // {
        //     if (context.performed)
        //     {
        //         rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //         IsJumping = true;
        //     }
        //     // 점프 조건 수정 필요
        //     else if(context.canceled/* 바닥에 착지했을 때 */)
        //     {
        //         IsJumping = false;
        //     }
        // }

        // 마우스 클릭 테스트
        // public void OnMouseClick(InputAction.CallbackContext context)
        // {
        //     if(context.started)
        //     {
        //         Debug.Log("마우스 클릭 감지됨");
        //     }
        // }
    }
}
