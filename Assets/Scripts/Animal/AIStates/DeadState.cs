using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    float delaysecond = 0f;
    public DeadState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Dead);
    }

    public void Exit()
    {
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if(delaysecond > 1.5f)
        {
            _Animals.States = AnimalAI.State.Idle;
            _AnimalStats.currentHealth = _AnimalStats.animalSO.health;
            _AnimalStats.health = _AnimalStats.animalSO.health;
            AnimalSpawner.Instance.InsertQueue(_Animals.gameObject);
            _Animals.Dead();
        }
    }


}
