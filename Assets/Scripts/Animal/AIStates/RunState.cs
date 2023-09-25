using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    public RunState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Run);
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Stay()
    {
        if (_Animals.IsDeadCheck(_AnimalStats))
        {
            _Animals.States = AnimalAI.State.Death;
        }
        throw new System.NotImplementedException();
    }
}
