using UnityEngine;
namespace My2D
{
    public class TouchingDirections : MonoBehaviour
    {
        #region Variables
        // 충돌을 감지할 캡슐 콜라이더
        private CapsuleCollider2D touchingCol;
        // 애니메이터 컴포넌트
        Animator animator;
        // 2D 물리 충돌을 감지하기 위한 필터
        [SerializeField] private ContactFilter2D castFilter;
        
        // 바닥, 벽, 천장과의 거리 설정
        [SerializeField] private float groundDistance = 0.05f;
        [SerializeField] private float wallDistance = 0.2f;
        [SerializeField] private float ceilingDistance = 0.05f;
        
        

        // 충돌 감지를 위한 RaycastHit2D 배열
        RaycastHit2D[] groundHits = new RaycastHit2D[5];
        RaycastHit2D[] wallHits = new RaycastHit2D[5];
        RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

        // 바닥에 닿아 있는지 여부
        [SerializeField] private bool _isGrounded;
        public bool IsGrounded 
        { 
            get => _isGrounded; 
            private set 
            { 
                _isGrounded = value; 
                // 애니메이터에 바닥에 닿아 있는지 여부를 설정
                animator.SetBool(AnimationString.IsGrounded, value); 
            } 
        }

        // 벽에 닿아 있는지 여부
        [SerializeField] private bool _isOnWall;
        public bool IsOnWall 
        { 
            get => _isOnWall; 
            private set 
            { 
                _isOnWall = value; 
                // 애니메이터에 벽에 닿아 있는지 여부를 설정
                animator.SetBool(AnimationString.IsOnWall, value); 
            } 
        }

        // 천장에 닿아 있는지 여부
        [SerializeField] private bool _isOnCeiling;
        public bool IsOnCeiling 
        { 
            get => _isOnCeiling; 
            private set 
            { 
                _isOnCeiling = value; 
                // 애니메이터에 천장에 닿아 있는지 여부를 설정
                animator.SetBool(AnimationString.IsOnCeiling, value); 
            } 
        }

        // 벽 체크 방향 (오른쪽 또는 왼쪽)
        private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        #endregion

        void Awake()
        {
            // 컴포넌트 초기화
            touchingCol = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            // 바닥, 벽, 천장과의 충돌 여부를 감지
            IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0f;
            IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0f;
            IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0f;
        }
    }
}
