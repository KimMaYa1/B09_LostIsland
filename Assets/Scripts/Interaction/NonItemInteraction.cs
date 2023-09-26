using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NonItemInteraction : MonoBehaviour, IInteractable
{

    public string GetInteractPrompt(string str)
    {
        return "상호작용";
        //return string.Format("{0} {1}",  ,item.itemName);
    }

    public void OnInteract()
    {
        Debug.Log("상호작용하였습니다");
    }
}
