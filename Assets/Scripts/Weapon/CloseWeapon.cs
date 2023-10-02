using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // 근접 무기 이름

    //근접 무기 종류 bool
    public bool isHand;
    public bool isOneHanded_Sword;
    public bool isTwoHanded_Sword;


    public float range; // 공격범위
    public float damage; // 공격력
    public float attackDelay; // 공격 딜레이(공격 속도)

    //무기를 다 휘두르면 공격 판정
    public float attackDelayStart; //공격 활성화
    //공격이 끝나는 시점
    public float attackDelayEnd; // 공격 비활성화 

    public Animator anim;

    //주먹 부분에 박스 콜라이더 추가(데미지 판정부분)
    public BoxCollider handColl;
}
