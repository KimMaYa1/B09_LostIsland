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

    [Header("Movemet")]
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public Transform target;
    public float camRotSpeed;
    private float camCurYRot;

    private Vector2 lookPhase;

    [HideInInspector]
    public PlayerController instance;
    public bool canLook = true;
    
    private Rigidbody _rigidbody;
    public float delayTime = 0;
    public bool IsAttackDelay = true;

    private void Awake()
    {
        instance = this;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        if (IsAttackDelay)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= playerStat.MoveSpeed;
        dir.y = _rigidbody.velocity.y;
        cameraContainer.position = transform.position;

        _rigidbody.velocity = dir;

        if (_rigidbody.velocity.x != 0 || _rigidbody.velocity.z != 0)
        {
            transform.LookAt(target);
        }
    }

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

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
                _rigidbody.AddForce(Vector2.up * playerStat.JumpForce, ForceMode.Impulse);
        }
    }
    
    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]        //앞 뒤 왼 오 에다가 ray만들어서 그라운드와 만나고있는지 확인
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (IsAttackDelay)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Debug.Log("a");
                IsAttackDelay = false;
            }
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

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
