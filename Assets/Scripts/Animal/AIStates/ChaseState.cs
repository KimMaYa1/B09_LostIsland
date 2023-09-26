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
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if (_Animals.IsDeadCheck(_AnimalStats))
        {
            _Animals.States = AnimalAI.State.Dead;
        }
        //30초 동안 쫓다가 안되면 Idle로 변경
        if(delaysecond >= 30f)
        {
            _Animals.States = AnimalAI.State.Idle;
        }
        //쫓는 것 구현  플레이어 위치 받아와야 함

        CheckArrival();
    }
    
    public void ChaseTarget(Transform transform)
    {
        
        _Animals.nav.SetDestination(transform.position);
        _isChase = true;
    }


    private void CheckArrival()
    {
        if (!_Animals.nav.pathPending && _Animals.nav.remainingDistance < 0.1f)
        {
            _isChase = false;
        }
        if (delaysecond >= 5 && !_isChase)
        {
            int rand = Random.Range(1, 5);
            Debug.Log("Run : " + rand);
            if (rand >= 3)
            {
                _Animals.States = AnimalAI.State.Idle;
            }
            delaysecond = 0;
        }
    }
}
