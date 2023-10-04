using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public AnimalStats _animalStats;
    public void Awake()
    {
        _animalStats.health = _animalStats.animalSO.health;
        _animalStats.currentHealth = _animalStats.animalSO.health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            GetHit();
        }
    }

   

    public void GetHit()
    {
        _animalStats.currentHealth -= 10;
    }

    public void Attack()
    {

    }

}
