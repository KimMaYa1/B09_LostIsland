using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeaponController : MonoBehaviour
{
    //현재 장착한 Hand 모션타입 무기
    [SerializeField]
    private CloseWeapon currentCloseWeapon;

    //공격 상태체크
    private bool isAttack = false;
    private bool isSwing = false;

    //공격 시 대상 체크
    private RaycastHit hitInfo;

    void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

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
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    //공격 딜레이(중복 실행 방지 : 공격 시도 성공->바로 업데이트 중지)
    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //여기 공격 애니메이션 실행
        //currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayStart);
        isSwing = true; //공격 판정 on

        //공격 시점(공격 판정 중 : 지속해서 적중 여부 판단)
        StartCoroutine(HitCoroutine());


        yield return new WaitForSeconds(currentCloseWeapon.attackDelayEnd);
        isSwing = false; //공격 판정 off

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayStart - currentCloseWeapon.attackDelayEnd);
        isAttack = false; // 공격 중off (공격 가능)
    }

    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                //주변에 대한 있음
            }
            else
            {
                //충돌 안함(주변에 대상 없음)
            }
            yield return null; 
        }
    }

    private bool CheckObject()
    {
        //공격 시도 시 충돌체(동물)가 있는지 확인 나중에 변경하거나 조정★(현재 위치/방향/받아올 충돌체/범위)
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    public void CloseWeaponChange(CloseWeapon closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
