using UnityEngine;
using UnityEngine.InputSystem;

namespace My2D
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        private Rigidbody2D rb2D;
        //플레이어 걷기 속도
        private float walkSpeed = 4f;
        //플레이어 이동과 관련된 입력값
        private Vector2 moveInput;
        //플레이어 점프 파워
        private float jumpForce = 5f;
        #endregion

        void Awake()
        {
            //참조
            rb2D = this.GetComponent<Rigidbody2D>();
            //rb2D.velocity

        }

        void FixedUpdate()
        {   
            //좌우 이동
            rb2D.velocity = new Vector2(moveInput.x * walkSpeed, rb2D.velocity.y);
        }

        //뉴 인풋 시스템 사용
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
            // Debug.Log("context : " + moveInput);
        }
        //점프 구현
        // public void OnJump(InputAction.CallbackContext context)
        // {
        //     if (context.performed && rb2D.velocity.y == 0)
        //     {
        //         rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //     }
        // }
    }

}
