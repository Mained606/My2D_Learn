using UnityEngine;
using UnityEngine.Events;

namespace My2D
{
    //캐릭터와 관련 된 이벤트 함수들을 관리하는 클래스
    public static class CharacterEvents
    {
        //캐릭터가 데미지를 입었을 때 호출되는 이벤트
        public static UnityAction<GameObject, float> characterDamaged;

        //캐릭터가 힐할 때 등록된 실수 함수 호출
        public static UnityAction<GameObject, float> characterHealed;

    }

}
