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
        throw new System.NotImplementedException();
    }

    public void Stay()
    {
        if (_Animals.IsDeadCheck(_AnimalStats))
        {
            _Animals.States = AnimalAI.State.Dead;
        }

        switch (_AnimalStats.animal)
        {
            case Animal.Bear:
            case Animal.Fox:
                _Animals.States = AnimalAI.State.GetHit;
                break;
            case Animal.Elephant:
            case Animal.Rhino:

            case Animal.Rabbit:
                if (!_isRun)
                {
                    RunAway();
                }
                break;
        }
        CheckArrival();
    }
    
    public void RunAway()
    {
        _randomPoint = _Animals.GetRandomPositionOnNavMesh();
        Debug.Log(_randomPoint);
        _Animals.nav.SetDestination(_randomPoint);
        _isRun = true;
    }

    public void RunToTarget(Transform direction)
    {

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
            Debug.Log("Run : " + rand);
            if (rand >= 3)
            {
                _Animals.States = AnimalAI.State.Idle;
            }
            delaysecond = 0;
        }
    }
}
