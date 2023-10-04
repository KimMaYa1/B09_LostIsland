using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    private bool _isRun = false;

    Vector3 _randomPoint;

    float delaysecond = 0f;
    public RunState(AnimalAI animalAI, AnimalStats animalStats)
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
        
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;

        if (_AnimalStats.currentHealth < _AnimalStats.health)
        {
            _AnimalStats.health = _AnimalStats.currentHealth;
        }

        if (!_isRun)
        {
            RunAway();
        }   
        CheckArrival();
    }

    public void RunAway()
    {
        Vector3 directionVector = _Animals.transform.position + new Vector3(5, 0, 5) - GameManager.Instance.PlayerObj.transform.position;

        // 방향 벡터를 정규화(normalize)
        directionVector.Normalize();

        // Rabbit을 움직일 방향 벡터 계산 (-90도에서 90도 사이)
        Vector3 moveDirection = Quaternion.Euler(0, Random.Range(-90f, 90f), 0) * directionVector;
        Vector3 runPoint = _Animals.transform.position + moveDirection * _Animals.animalStats.animalSO.range;
        _Animals.nav.SetDestination(runPoint);
        _isRun = true;
    }

    private void CheckArrival()
    {
        if (!_Animals.nav.pathPending && _Animals.nav.remainingDistance < 0.1f)
        {
            _isRun = false;
        }
        if (delaysecond >= 5 && !_isRun)
        {
            int rand = Random.Range(1, 5);
            Debug.Log("Run : "+rand);
            if (rand >= 2)
            {
                _Animals.States = AnimalAI.State.Idle;
            }
            delaysecond = 0;
        }
    }
}
