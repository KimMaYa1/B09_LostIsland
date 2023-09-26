using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Animal
{
    Bear,
    Rabbit,
    Fox,
    Elephant,
    Rhino
}
public class AnimalStats : MonoBehaviour
{
    public AnimalSO animalSO;
    [SerializeField]
    public Animal animal;
}
