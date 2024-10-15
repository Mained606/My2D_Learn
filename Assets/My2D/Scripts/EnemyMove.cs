using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace My2D
{
    public class EnemyMove : MonoBehaviour
    {
        // 컴포넌트
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirections touchingDirections;
        //플레이어 감지
        public DetectionZone detectionZone;
        // 변수
        [SerializeField] private float runSpeed = 4f;
        //감속 계수
        [SerializeField] private float stopRate = 0.2f;
        // 방향
        private Vector2 directionVector = Vector2.right;
        //이동 가능 방향
        public enum WalkableDirection { Left, Right }
        //현재 이동 방향
        private WalkableDirection walkDirection = WalkableDirection.Right;
        public WalkableDirection WalkDirection
        {
            get
            {
                return walkDirection;
            }
            private set
            {
                //이미지 플립
                transform.localScale *= new Vector2(-1, 1);
                //실제 이동하는 방향값
                if (value == WalkableDirection.Left)
                {
                    directionVector = Vector2.left;
                }
                else if (value == WalkableDirection.Right)
                {
                    directionVector = Vector2.right;
                }

                walkDirection = value;
            }
        }

        //공격 타겟 설정
        [SerializeField] private bool hasTarget = false;
        public bool HasTarget
        {
            get { return hasTarget; }
            private set {
                hasTarget = value;
                animator.SetBool(AnimationString.HasTarget, value);
            }
        }

        //이동 가능 상태/불가능 상태 - 이동 제한
        public bool CanMove
        {
            get { return animator.GetBool(AnimationString.CanMove); }
        }


        void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = this.GetComponent<TouchingDirections>();
        }
        void Update()
        {
            HasTarget = (detectionZone.detectedColliders.Count > 0);
        }

        void FixedUpdate()
        {
            Move();
        }

        void Move()
        {
            //땅에서 이동시 벽을 만나면 방향 전환
            if(touchingDirections.IsOnWall && touchingDirections.IsGrounded)
            {
                //방향 전환 반전
                Flip();
            }
            if(CanMove)
            {
                rb2D.velocity = new Vector2(directionVector.x * runSpeed, rb2D.velocity.y);
            }
            else
            {
                //rb2D.velocity.x -> 0 : Lerp
                rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x, 0f, stopRate), rb2D.velocity.y);
            }
        }

        //방향전환 반전
        private void Flip()
        {
            if(WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if(WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.LogError("Invalid direction");
            }
        }

        // void Move()
        // {
        //     // 벽에 닿아 있지 않고 바닥에 닿아 있으면 오브젝트를 오른쪽으로 walkspeed만큼 이동
        //     if(touchingDirections.IsGrounded && !touchingDirections.IsOnWall)
        //     {
        //         IsFacingRight = true;
        //         rb2D.velocity = new Vector2(walkSpeed, rb2D.velocity.y);
        //     }
        //     else
        //     {
        //         IsFacingRight = false;
        //         rb2D.velocity = new Vector2(-walkSpeed, rb2D.velocity.y);
        //     }

        //     animator.SetFloat(AnimationString.yVelocity, rb2D.velocity.y);
        // }
    }

}
