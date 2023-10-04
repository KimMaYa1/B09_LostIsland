using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using static UnityEditor.Timeline.TimelinePlaybackControls;

//아이템 레이 쏴서 먹으면 아웃으로 아이템 스크립트정보를 받아오기

public class PlayerController : MonoBehaviour
{
    public PlayerStat playerStat;

    [Header("Test")]
    public Collider attackCollider;

    /*[Header("Movemet")]
    private Vector2 curMovementInput;*/
    [Header("Jump")]
    public float maxJumpRange;

    [HideInInspector]
    public static PlayerController instance;
    public PlayerConditins playerConditins;
    public string _str;

    public float delayTime = 0;
    public bool IsAttackDelay = true;
    public bool canWater = false;

    private Animator animator;

    private void Awake()
    {
        playerStat.Atk = 50;
        playerStat.Def = 10;
        attackCollider.enabled = false;
        instance = this;
        animator = GetComponentInChildren<Animator>();
        playerConditins = GetComponent<PlayerConditins>();
        playerConditins.stat = playerStat;
    }

    private void FixedUpdate()
    {
        if(delayTime >= 0.55f)
        {
            IsAttackDelay = true;
            delayTime = 0;
        }
        else if (!IsAttackDelay)
        {
            delayTime += Time.fixedDeltaTime;
        }
        /*if (IsAttackDelay)
        {
            Move();
        }*/
    }

    

    /*private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= playerStat.MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        if (_rigidbody.velocity.x != 0 || _rigidbody.velocity.z != 0)
        {
            transform.LookAt(target);
        }
    }*/

    

    /*public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            isClickMove = false;
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }*/

    public void AttackInvoke()
    {
        attackCollider.enabled = false;
        animator.SetBool("IsAttack", false);
    }

    public void OnAttackInput(string str)
    {
        _str = str;
        animator.SetBool("IsAttack", true);
        if ("한손검" == _str)
        {
            animator.SetTrigger("AttackOneHanded");
        }
        else if ("양손검" == _str)
        {
            animator.SetTrigger("AttackTwoHanded");
        }
        else
        {
            if (Random.RandomRange(0, 2) == 0)
            {
                animator.SetTrigger("AttackPunchRight");
            }
            else
            {
                animator.SetTrigger("AttackPunchLeft");
            }
        }


        IsAttackDelay = false;
        attackCollider.enabled = true;
        Invoke("AttackInvoke", 0.5f);
    }

    public void OnDrinkWater(InputAction.CallbackContext context)
    {
        if (canWater && context.phase == InputActionPhase.Started)
        {
            playerConditins.Drink(30);
            GameObject waterbottle = Resources.Load<GameObject>("Water_Bottle");
            Instantiate(waterbottle, this.transform.position + Vector3.up, Quaternion.identity);
        }
    }
}
