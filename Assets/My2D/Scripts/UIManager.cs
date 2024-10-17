using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

namespace My2D
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        public GameObject damageTextPrefab;
        public GameObject healthTextPrefab;
        private Canvas canvas;
        [SerializeField] private Vector3 healthOffset = Vector3.zero;

        public float health;
        public float currentHealth;
        
        #endregion

        void Awake()
        {   
            //캔버스 참조
            canvas = FindObjectOfType<Canvas>();
        }

        private void OnEnable()
        {
            //캐릭터 관련 이벤트 함수 등록
            CharacterEvents.characterDamaged += CharacterTakeDamage;
            CharacterEvents.characterHealed += CharacterHeal;
        }

        private void OnDisable()
        {
            //캐릭터 관련 이벤트 함수 등록 해제
            CharacterEvents.characterDamaged -= CharacterTakeDamage;
            CharacterEvents.characterHealed -= CharacterHeal;
        }

        public void CharacterTakeDamage(GameObject character, float damage)
        {
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
            //데미지 텍스트 생성
            GameObject textGO = Instantiate(damageTextPrefab, spawnPosition + healthOffset, Quaternion.identity, canvas.transform);
            TextMeshProUGUI damageText = textGO.GetComponent<TextMeshProUGUI>();
            damageText.text = damage.ToString();
        }

        public void CharacterHeal(GameObject character, float amount)
        {
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
            //힐 텍스트 생성
            GameObject textGO = Instantiate(healthTextPrefab, spawnPosition + healthOffset, Quaternion.identity, canvas.transform);
            TextMeshProUGUI healthText = textGO.GetComponent<TextMeshProUGUI>();
            
            //2024-10-17 수정
            healthText.text = currentHealth.ToString();
        }   

        //2024-10-17 추가
        // 현재 체력 회복량 받아오기
        public void GetCurrentHealth(float amount)
        {
            currentHealth = amount;
            Debug.Log("받아온 체력값: "+ currentHealth);
        }

    }
}

