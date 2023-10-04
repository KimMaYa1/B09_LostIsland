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
        _AnimalStats.currentHealth = _AnimalStats.animalSO.health;
        _AnimalStats.health = _AnimalStats.animalSO.health;
    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if(delaysecond > 3f)
        {
            delaysecond = 0;
            _Animals.States = AnimalAI.State.Idle;
            AnimalSpawner.Instance.InsertQueue(_Animals.gameObject);
            _Animals.Dead();
        }
    }


}
