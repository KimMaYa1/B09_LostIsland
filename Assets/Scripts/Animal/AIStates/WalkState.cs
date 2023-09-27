using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    private bool _isWalk = false;
    Vector3 _randomPoint;

    float delaysecond = 0f;
    public WalkState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Walk);
        _Animals.nav.speed = _AnimalStats.animalSO.moveSpeed;
    }

    public void Exit()
    {
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        // 공격 받았을 때
        if (_AnimalStats.animalSO.currentHealth < _AnimalStats.animalSO.health)
        {
            _AnimalStats.animalSO.health = _AnimalStats.animalSO.currentHealth;
            switch (_AnimalStats.animal)
            {
                case Animal.Bear:
                case Animal.Fox:
                    _Animals.States = AnimalAI.State.GetHit;
                    break;
                case Animal.Elephant:
                case Animal.Rhino:
                    _Animals.States = AnimalAI.State.Chase;
                    break;
                case Animal.Rabbit:
                    _Animals.States = AnimalAI.State.Run;
                    break;
            }
        }
        // 그냥 걸어다니는 것
        if (!_isWalk)
        {
            StartWalking(); // 걷기 시작
        }

        // 도착했을 때 체크
        CheckArrival();
    }

    // 걷는 동작을 처리하는 함수
    private void StartWalking()
    {
        _randomPoint = _Animals.GetRandomPositionOnNavMesh();
        Debug.Log(_randomPoint);
        _Animals.nav.SetDestination(_randomPoint);
        _isWalk = true;
    }

    // 목적지 도착을 체크하고 필요에 따라 상태 변경하는 함수
    private void CheckArrival()
    {
        if (!_Animals.nav.pathPending && _Animals.nav.remainingDistance < 0.1f)
        {
            _isWalk = false;
        }
        if (delaysecond >= 5 && !_isWalk)
        {
            int rand = Random.Range(1, 5);
            Debug.Log("Walk : " + rand);
            if (rand >= 3)
            {
                _Animals.States = AnimalAI.State.Idle;
            }
            delaysecond = 0;
        }
    }
}
