using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    public class ParallaxEffect : MonoBehaviour
    {
        #region Variables
        public Camera camera; //카메라
        public Transform followTarget; //플레이어
        //시작 위치
        private Vector2 startingPosition; //시작 위치 (배경, 카메라)
        private float startingZ; //시작할 때 배경의 z축 위치값

        //시작지점으로 부터의 카메라가 있는 위치까지의 거리
        private Vector2 CamMoveSinceStart => startingPosition - (Vector2)camera.transform.position;
        //배경과 플레이어와의 z축 거리
        private float zDistanceFormTarget => transform.position.z - followTarget.position.z;
        //
        private float ClippingPlane => camera.transform.position.z + (zDistanceFormTarget > 0 ? camera.farClipPlane : camera.nearClipPlane);
        //시차 거리 factor
        private float ParallaxFactor => Mathf.Abs(zDistanceFormTarget) / ClippingPlane;
        #endregion

        void Start()
        {
            //초기화
            startingPosition = transform.position;
            startingZ = transform.position.z;
        }

        void Update()
        {
            Vector2 newPosition = startingPosition + CamMoveSinceStart * ParallaxFactor;
            transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
        }
    }

}
