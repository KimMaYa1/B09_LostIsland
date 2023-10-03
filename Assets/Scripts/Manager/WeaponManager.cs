using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //���� �ߺ� ��ü ���� ����
    public static bool isChangeWeapon = false;

    //���� ���� & ���� ������ ���
    //���� ���⸦ Transform���� �޴� ����? ���� �Ѵ� ���Ҹ� �ϴ� �����̹Ƿ�
    //�� ������ ��ũ��Ʈ���� �����ϴ� ��� ��� ������ �����ؾ���
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    //���� ������ Ÿ��(����, ���Ÿ�)
    [SerializeField]
    private string currentWeaponType;

    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //�Ǽ��� ������ ���� �����(�ܼ��� Į/������ ���..)
    [SerializeField]
    private CloseWeapon[] closeWeapons;

    //���� �߰��� �ش� ������ �迭 : ��ũ��Ʈ �߰�(Ȱ/��/���� ���..)

    //���� ���� ��ųʸ�
    //Ű : ���� / ��� : ���� ���� class
    private Dictionary<string, CloseWeapon> closeWeaponDictionary = new Dictionary<string, CloseWeapon>();

 

    //�ʿ��� ������Ʈ
    [SerializeField]
    private CloseWeaponController closeWeaponController;


    void Start()
    {
        //��ųʸ� �߰�(���� ���� �߰� ��) �ش� �ݺ��� �߰�
        for (int i = 0; i < closeWeapons.Length; i++)
        {
            closeWeaponDictionary.Add(closeWeapons[i].closeWeaponName, closeWeapons[i]);
        }
    }


    //���� ��ü(�ӽ�/���� �ʿ�)
    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ѽհ�"));
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("���ⱳü ���"); //�ʿ� ������ ����

        yield return new WaitForSeconds(changeWeaponDelayTime);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type; //�ٲ� ���� Ÿ��(�ʿ� ������ ����-���Ÿ� �����)

        //���ⱳü ���� on
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
