using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NonItemInteraction : MonoBehaviour, IInteractable
{

    public string GetInteractPrompt(string str)
    {
        return "��ȣ�ۿ�";
        //return string.Format("{0} {1}",  ,item.itemName);
    }

    public void OnInteract()
    {
        Debug.Log("��ȣ�ۿ��Ͽ����ϴ�");
    }
}
