using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            
            healthText.text = amount.ToString();
        }   

    }
}

