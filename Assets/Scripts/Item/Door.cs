using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;

    bool isOpen = false;

    public void InteractionDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("open",isOpen);
    }
}
