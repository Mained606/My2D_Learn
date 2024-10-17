using UnityEngine;
using TMPro;

namespace My2D
{
    public class HealthText : MonoBehaviour
    {
        #region Variables
        private TextMeshProUGUI healthText;
        private RectTransform textTransform;

        //이동
        [SerializeField] private float moveSpeed = 100f;

        //페이드 효과
        private Color startColor;
        public float fadeTime = 1f;
        private float countdown = 0f;
        #endregion

        void Awake()
        {
            //참조
            healthText = GetComponent<TextMeshProUGUI>();
            textTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            //초기화
            startColor = healthText.color;
            countdown = fadeTime;
        }

        //체력 텍스트 설정

        void Update()
        {
            
            //이동 
            textTransform.position += Vector3.up * moveSpeed * Time.deltaTime;

            //페이드 효과
            countdown -= Time.deltaTime;
            if(countdown > 0)
            {
                float newAlpha = startColor.a * (countdown / fadeTime);
                healthText.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            }
            //페이드 타임 끝
            if(countdown <= 0)
            {
                //페이드 효과 종료 후 킬
                Destroy(gameObject);
            }       
        }
    }
}
