using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    //현재 무기 & 현재 무기의 모션
    //현재 무기를 Transform으로 받는 이유? 껏다 켜는 역할만 하는 변수이므로
    //각 무기의 스크립트에서 실행하는 경우 모든 변수를 선언해야함
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    //현재 무기의 타입(근접, 원거리)
    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //맨손을 포함한 근접 무기들(단순한 칼/몽둥이 등등..)
    [SerializeField]
    private CloseWeapon[] closeWeapons;

    //무기 추가시 해당 무기의 배열 : 스크립트 추가(활/총/마법 등등..)

    //무기 관리 딕셔너리
    //키 : 문자 / 밸류 : 근접 무기 class
    private Dictionary<string, CloseWeapon> closeWeaponDictionary = new Dictionary<string, CloseWeapon>();

 

    //필요한 컴포넌트
    [SerializeField]
    private CloseWeaponController closeWeaponController;


    void Start()
    {
        //딕셔너리 추가(무기 유형 추가 시) 해당 반복도 추가
        for (int i = 0; i < closeWeapons.Length; i++)
        {
            closeWeaponDictionary.Add(closeWeapons[i].closeWeaponName, closeWeapons[i]);
        }
    }


    //무기 교체(임시/수정 필요)
    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "한손검"));
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("무기교체 모션"); //필요 없으면 제거

        yield return new WaitForSeconds(changeWeaponDelayTime);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type; //바뀐 무기 타입(필요 없으면 제거-원거리 무기용)

        //무기교체 가능 on
        isChangeWeapon = false; 

    }

    private void WeponChange(string type, string name)
    {
        if(type == "HAND")
        {
            closeWeaponController.CloseWeaponChange(closeWeaponDictionary[name]);
        }
    }
}
