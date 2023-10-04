using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class InteractionTarget : MonoBehaviour
{
    public LayerMask layerMask;

    [HideInInspector]
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) | layerMask) == layerMask)
        {
            target = other.gameObject;
        }
        if (other.tag == "AnimalWeapon")
        {
            PlayerController.instance.playerConditins.TakePhysicalDamage(10);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) | layerMask) == layerMask)
        {
            target = other.gameObject;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
