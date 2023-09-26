using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemInteraction : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt(string str)
    {
        return string.Format("ащ╠Б {0}", str);
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }
}
