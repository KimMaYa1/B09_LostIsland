using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAttack : MonoBehaviour
{
    PlayerConditins playerConditins;
    AnimalStats aniamlStats;
    private void Awake()
    {
        playerConditins = GameManager.Instance.PlayerObj.GetComponentInChildren<PlayerConditins>();
        aniamlStats = this.gameObject.GetComponentInParent<AnimalStats>();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Debug.Log(playerConditins);
    //        playerConditins.TakePhysicalDamage((int)aniamlStats.animalSO.power);
    //    }
    //}
}
