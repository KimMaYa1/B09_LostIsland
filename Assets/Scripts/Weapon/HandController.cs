using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //���� ������ Hand ���Ÿ�� ����
    [SerializeField]
    private Hand currentHand;

    //���� ����üũ
    private bool isAttack = false;
    private bool isSwing = false;

    //���� �� ��� üũ
    private RaycastHit hitInfo;



    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        //���� �õ� ����(���� �Է��� ���Դ��� üũ && ��� ����� ������ ������ üũ)
        //�ӽ�(��Ŭ��)->��ǲ �ý��� ���, ��� ���� üũ �߰��Ұ�
        if(Input.GetButton("fire1"))
        {
            //���� �� �ƴ�(������ ��� ����)
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
        isSwing = true; //���� ���� on


        yield return new WaitForSeconds(currentHand.attackDelayEnd);
        isSwing = false; //���� ���� off

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayStart - currentHand.attackDelayEnd);
        isAttack = false; // ���� ��off (���� ����)
    }
}
