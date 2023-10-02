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
        _Animals.PlayAnimation(AnimalAI.State.Attack);
    }

    public void Exit()
    {

    }

    public void Stay()
    {
        delaysecond += Time.deltaTime;
        if (Vector3.Distance(_Animals.transform.position, GameManager.Instance.PlayerObj.transform.position) >= _AnimalStats.animalSO.attackRange && delaysecond > 2f)
        {
            _Animals.States = AnimalAI.State.Chase;
        }
    }
}
