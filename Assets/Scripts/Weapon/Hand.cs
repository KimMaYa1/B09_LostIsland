using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public string handName; // �Ǽ�or�Ǽ����(���� ���)
    public float range; // �Ǽշ� ���ݹ���
    public float damage; // �Ǽշ� ���ݷ�
    public float attackDelay; // ���� ������(���� �ӵ�)

    //���⸦ �� �ֵθ��� ���� ����
    public float attackDelayStart; //���� Ȱ��ȭ
    //������ ������ ����
    public float attackDelayEnd; // ���� ��Ȱ��ȭ 

    public Animator anim;

    //�ָ� �κп� �ڽ� �ݶ��̴� �߰�(������ �����κ�)
    public BoxCollider handColl;
}
