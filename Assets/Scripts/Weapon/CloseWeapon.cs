using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName; // ���� ���� �̸�

    //���� ���� ���� bool
    public bool isHand;
    public bool isOneHanded_Sword;
    public bool isTwoHanded_Sword;


    public float range; // ���ݹ���
    public float damage; // ���ݷ�
    public float attackDelay; // ���� ������(���� �ӵ�)

    //���⸦ �� �ֵθ��� ���� ����
    public float attackDelayStart; //���� Ȱ��ȭ
    //������ ������ ����
    public float attackDelayEnd; // ���� ��Ȱ��ȭ 

    public Animator anim;

    //�ָ� �κп� �ڽ� �ݶ��̴� �߰�(������ �����κ�)
    public BoxCollider handColl;
}
