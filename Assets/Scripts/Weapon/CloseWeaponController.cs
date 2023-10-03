using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeaponController : MonoBehaviour
{
    //���� ������ Hand ���Ÿ�� ����
    [SerializeField]
    private CloseWeapon currentCloseWeapon;

    //���� ����üũ
    private bool isAttack = false;
    private bool isSwing = false;

    //���� �� ��� üũ
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
        //���� �õ� ����(���� �Է��� ���Դ��� üũ && ��� ����� ������ ������ üũ)
        //�ӽ�(��Ŭ��)->��ǲ �ý��� ���, ��� ���� üũ �߰��Ұ�
        if(Input.GetButton("fire1"))
        {
            //���� �� �ƴ�(������ ��� ����)
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    //���� ������(�ߺ� ���� ���� : ���� �õ� ����->�ٷ� ������Ʈ ����)
    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //���� ���� �ִϸ��̼� ����
        //currentCloseWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayStart);
        isSwing = true; //���� ���� on

        //���� ����(���� ���� �� : �����ؼ� ���� ���� �Ǵ�)
        StartCoroutine(HitCoroutine());


        yield return new WaitForSeconds(currentCloseWeapon.attackDelayEnd);
        isSwing = false; //���� ���� off

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayStart - currentCloseWeapon.attackDelayEnd);
        isAttack = false; // ���� ��off (���� ����)
    }

    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                //�ֺ��� ���� ����
            }
            else
            {
                //�浹 ����(�ֺ��� ��� ����)
            }
            yield return null; 
        }
    }

    private bool CheckObject()
    {
        //���� �õ� �� �浹ü(����)�� �ִ��� Ȯ�� ���߿� �����ϰų� ������(���� ��ġ/����/�޾ƿ� �浹ü/����)
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
