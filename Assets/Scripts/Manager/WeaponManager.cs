using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //맨손을 포함한 근접 무기들(단순한 칼/몽둥이 등등..)
    [SerializeField]
    private Hand[] hands;

    //무기 추가시 배열, 스크립트 추가(활/총/마법 등등..)
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
