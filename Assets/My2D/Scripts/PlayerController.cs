using My2d;
using UnityEngine;
using UnityEngine.InputSystem;

namespace My2D
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        private Animator animator;
        // private SpriteRenderer spriteRenderer;
        //플레이어 걷기 속도
        public float walkSpeed = 4f;
        //플레이어 뛰기 속도
        public float runSpeed = 8f;

        //플레이어 상태에 따라 이동속도 변경
        public float CurrentMoveSpeed
        {
            get
            {
                if (IsMoving)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    //정지
                    return 0;
                }
            }
        }
        //플레이어 이동과 관련된 입력값
        private Vector2 moveInput;

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
        //플레이어 달리기 여부
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

        // //플레이어 점프 여부
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
        //플레이어 좌우 방향 결정
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
            

        //플레이어 점프 파워
        private float jumpForce = 5f;

        #endregion

        void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();

            // spriteRenderer = this.GetComponent<SpriteRenderer>();
            //rb2D.velocity
        }

        void FixedUpdate()
        {   
            //좌우 이동
            rb2D.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb2D.velocity.y);
        }

        //뉴 인풋 시스템 사용
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            // Debug.Log("context : " + moveInput);
            //이동 여부 판단
            IsMoving = moveInput != Vector2.zero;

            SetPlayerFlip(moveInput);
        }

        //플레이어 좌우 방향 결정
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



         //달리기 구현
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
        
        // //점프 구현 - (무한 점프 수정 필요)
        // public void OnJump(InputAction.CallbackContext context)
        // {
        //     if (context.performed)
        //     {
        //         rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //         IsJumping = true;
        //     }
        //     //점프 조건 수정 필요
        //     else if(context.canceled/* 바닥에 착지했을 때 */)
        //     {
        //         IsJumping = false;
        //     }
        // }

        //마우스 클릭 테스트
        // public void OnMouseClick(InputAction.CallbackContext context)
        // {
        //     if(context.started)
        //     {
        //         Debug.Log("마우스 클릭 감지됨");
        //     }
        // }

    }
}   
