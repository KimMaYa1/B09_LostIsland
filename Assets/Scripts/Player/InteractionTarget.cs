using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class InteractionTarget : MonoBehaviour
{
    public LayerMask layerMask;

    [HideInInspector]
    public GameObject target;

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) | layerMask) == layerMask)
        {
            target = other.gameObject;
        }
        else
        {
            target = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        target = null;
    }
}
