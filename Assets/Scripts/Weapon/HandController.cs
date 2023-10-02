using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착한 Hand 모션타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격 상태체크
    private bool isAttack = false;
    private bool isSwing = false;

    //공격 시 대상 체크
    private RaycastHit hitInfo;



    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        //공격 시도 조건(공격 입력이 들어왔는지 체크 && 대상에 충분히 가까이 갔는지 체크)
        //임시(좌클릭)->인풋 시스템 사용, 대상 접근 체크 추가할것
        if(Input.GetButton("fire1"))
        {
            //공격 중 아님(딜레이 계산 시작)
            if(!isAttack)
            {

            }
        }
    }


    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayStart);
        isSwing = true; //공격 판정 on


        yield return new WaitForSeconds(currentHand.attackDelayEnd);
        isSwing = false; //공격 판정 off

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayStart - currentHand.attackDelayEnd);
        isAttack = false; // 공격 중off (공격 가능)
    }
}
