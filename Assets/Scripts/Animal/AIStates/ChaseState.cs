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
        //30초 동안 쫓다가 안되면 Idle로 변경
        if(delaysecond >= 30f || Vector3.Distance(_Animals.transform.position,GameManager.Instance.PlayerObj.transform.position) >= _AnimalStats.animalSO.range)
        {
            _Animals.States = AnimalAI.State.Idle;
        }
        //쫓는 것 구현  플레이어 위치 받아와야 함
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

        // 레이캐스트 시작 지점
        Vector3 startPoint = _Animals.transform.position;

        // 부채꼴 영역에 레이캐스트를 쏩니다.
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
