using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : IState
{
    private AnimalAI _Animals;
    private AnimalStats _AnimalStats;
    public DeadState(AnimalAI animalAI, AnimalStats animalStats)
    {
        _Animals = animalAI;
        _AnimalStats = animalStats;
    }
    public void Enter()
    {
        _Animals.PlayAnimation(AnimalAI.State.Death);
    }

    public void Exit()
    {
    }

    public void Stay()
    {
        //¸ó½ºÅÍ Á×À½
        _Animals.DeadAnimal();
    }
}
