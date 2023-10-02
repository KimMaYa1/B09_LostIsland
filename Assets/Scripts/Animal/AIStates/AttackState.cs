using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;

    float delaysecond = 0f;
    public AttackState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        Debug.Log("Attack Enter!");
        _Animals.PlayAnimation(AnimalAI.State.Attack);
        _Animals.nav.isStopped = true;
    }

    public void Exit()
    {
        _Animals.nav.isStopped = false;
        delaysecond = 0f;
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if (delaysecond > 1.0f)
        {
            _Animals.States = AnimalAI.State.Chase;
        }
    }
}
