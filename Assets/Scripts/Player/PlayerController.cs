using System.Collections;
using System.Collections.Generic;
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

    public float delayTime = 0;
    public bool IsAttackDelay = true;
    public bool canWater = false;

    private void Awake()
    {
        attackCollider.enabled = false;
        instance = this;
        playerConditins = GetComponent<PlayerConditins>();
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
        Debug.Log("b");
    }

    public void OnAttackInput()
    {
        Debug.Log("a");
        IsAttackDelay = false;
        attackCollider.enabled = true;
        Invoke("AttackInvoke", 0.5f);
    }

    public void OnDrinkWater(InputAction.CallbackContext context)
    {
        if (canWater)
        {
            playerConditins.Drink(30);
        }
    }
}
