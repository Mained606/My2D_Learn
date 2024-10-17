using UnityEngine;

namespace My2D
{
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera mainCamera; // 카메라
        public Transform followTarget; // 플레이어
        private Vector2 startingPosition; // 시작 위치 (배경, 카메라)
        private float startingZ; // 시작할 때 배경의 z축 위치값

        // 시작지점으로부터 카메라가 있는 위치까지의 거리
        private Vector2 CamMoveSinceStart => startingPosition - (Vector2)mainCamera.transform.position;
        // 배경과 플레이어와의 z축 거리
        private float zDistanceFromTarget => transform.position.z - followTarget.position.z;
        // 클리핑 평면 계산
        private float ClippingPlane => mainCamera.transform.position.z + (zDistanceFromTarget > 0 ? mainCamera.farClipPlane : mainCamera.nearClipPlane);
        // 시차 거리 factor
        private float ParallaxFactor => Mathf.Abs(zDistanceFromTarget) / ClippingPlane;
        #endregion

        void Start()
        {
            // 초기화
            startingPosition = transform.position;
            startingZ = transform.position.z;
        }

        void Update()
        {
            // 새로운 위치 계산
            Vector2 newPosition = startingPosition + CamMoveSinceStart * ParallaxFactor;
            transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
        }
    }
}
