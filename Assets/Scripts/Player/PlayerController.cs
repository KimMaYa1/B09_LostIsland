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

    /*[Header("Movemet")]
    private Vector2 curMovementInput;*/

    [Header("Look")]
    public Transform cameraContainer;
    public Transform target;
    public float camRotSpeed;
    private float camCurYRot;


    private Vector2 lookPhase;

    [HideInInspector]
    public static PlayerController instance;
    public bool canLook = true;

    public float delayTime = 0;
    public bool IsAttackDelay = true;

    private void Awake()
    {
        instance = this;
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

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
            cameraContainer.position = transform.position;
        }
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

    void CameraLook()
    {
        camCurYRot += lookPhase.x * camRotSpeed * Time.deltaTime;  
        cameraContainer.localEulerAngles = new Vector3(0, -camCurYRot, 0);
        //transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitiveity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if( context.phase == InputActionPhase.Performed)
        {
            lookPhase = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            lookPhase = Vector2.zero;
        }
    }

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

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (IsAttackDelay)
        {
            if (context.phase == InputActionPhase.Started)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                IsAttackDelay = false;
            }
        }
    }

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            
        }
    }
}
