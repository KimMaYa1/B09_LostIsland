using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;

    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //�Ǽ��� ������ ���� �����(�ܼ��� Į/������ ���..)
    [SerializeField]
    private Hand[] hands;

    //���� �߰��� �迭, ��ũ��Ʈ �߰�(Ȱ/��/���� ���..)
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
