using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    //게임 스프라이트 오브젝트를 페이드아웃 후 킬
    public class FadeRemoveBehaviour : StateMachineBehaviour
    {
        #region Variables
        //
        private SpriteRenderer spriteRenderer;
        private GameObject removeObject;
        private Color startColor;

        //fade 효과
        [SerializeField] private float fadeTime = 1f;
        private float countdown = 0f;

        //딜레이 시간 후에 fade 효과 처리
        public float delayTime = 2f;
        private float delayCountdown = 0f;

        //
        #endregion
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            startColor = spriteRenderer.color;
            removeObject = animator.gameObject;

            //초기화
            countdown = fadeTime;

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //delayTime 만큼 딜레이
            if(delayCountdown < delayTime)
            {
                delayCountdown += Time.deltaTime;
                return;
            }

            //페이드 효과 spriteRenderer.color.a : 1 -> 0
            countdown -= Time.deltaTime;
            
            //페이드 효과 계산 (*알파값이 불투명인 오브젝트의 경우를 위해 스타트 알파값을 곱해준다.)
            float newAlpha = startColor.a * (countdown / fadeTime);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

            //페이드 효과 종료 후 오브젝트 파괴
            if(countdown <= 0)
            {
                Destroy(removeObject);
            }

        }


    }
}
