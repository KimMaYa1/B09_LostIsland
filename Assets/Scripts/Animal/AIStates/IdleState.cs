using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;

    float delaysecond = 0f;
    public IdleState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Idle);
    }

    public void Exit()
    {
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        // 공격 받았을 때
        if (_AnimalStats.currentHealth < _AnimalStats.health)
        {
            _AnimalStats.health = _AnimalStats.currentHealth;
            switch (_AnimalStats.animal)
            {
                case Animal.Bear:
                case Animal.Fox:
                    _Animals.States = AnimalAI.State.GetHit;
                    break;
                case Animal.Elephant:
                case Animal.Rhino:
                    _Animals.States = AnimalAI.State.Attack;
                    break;
                case Animal.Rabbit:
                    _Animals.States = AnimalAI.State.Run;
                    break;
            }
        }
        if(delaysecond >= 1)
        {
            int rand = Random.Range(1, 5);
            if (rand >= 3)
            {
                _Animals.States = AnimalAI.State.Walk;
            }
            delaysecond = 0;
        }

    }
}
