using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    float delaysecond = 0f;
    public GetHitState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.GetHit);
    }

    public void Exit()
    {

    }

    public void Stay()
    {
        delaysecond = Time.deltaTime;
        if(delaysecond >= 0.1f)
        {
            if (_AnimalStats.animal == Animal.Fox)
            {
                int rand = Random.Range(1, 5);
                if (rand >= 3)
                {
                    _Animals.States = AnimalAI.State.Run;
                }
                else
                {
                    _Animals.States = AnimalAI.State.Chase;
                }
            }
            else
            {
                _Animals.States = AnimalAI.State.Chase;
            }
        }
    }

}
