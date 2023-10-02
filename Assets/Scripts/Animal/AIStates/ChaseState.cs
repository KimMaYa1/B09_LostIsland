using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    private bool _isChase = false;


    float delaysecond = 0f;
    public ChaseState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Run);
        _Animals.nav.speed = _AnimalStats.animalSO.runSpeed;
    }

    public void Exit()
    {
        _isChase = false;
        delaysecond = 0f;
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        ChaseTarget();
        CheckArrival();
        //30�� ���� �Ѵٰ� �ȵǸ� Idle�� ����
        if(delaysecond >= 30f || Vector3.Distance(_Animals.transform.position,GameManager.Instance.PlayerObj.transform.position) >= _AnimalStats.animalSO.range)
        {
            _Animals.States = AnimalAI.State.Idle;
        }
        //�Ѵ� �� ����  �÷��̾� ��ġ �޾ƿ;� ��
    }
    
    public void ChaseTarget()
    {
        _Animals.nav.SetDestination(GameManager.Instance.PlayerObj.transform.position);
    }


    private void CheckArrival()
    {
        float halfAngle = 45 * 0.5f;
        Vector3 forward = _Animals.transform.forward;
        Vector3 leftDirection = Quaternion.Euler(0, -halfAngle, 0) * forward;

        // ����ĳ��Ʈ ���� ����
        Vector3 startPoint = _Animals.transform.position;

        // ��ä�� ������ ����ĳ��Ʈ�� ���ϴ�.
        RaycastHit[] hits = Physics.SphereCastAll(startPoint, 0.1f, forward, _AnimalStats.animalSO.attackRange);

        foreach (RaycastHit hit in hits)
        {
            Vector3 hitDirection = hit.point - startPoint;
            float angleToHit = Vector3.Angle(leftDirection, hitDirection);
            if (angleToHit <= halfAngle)
            {
                if(((1 << hit.collider.gameObject.layer) | _Animals.playerLayerMask) == _Animals.playerLayerMask)
                {
                    _Animals.States = AnimalAI.State.Attack;
                }
            }
        }
    }
}
