using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamMove : MonoBehaviour
{
    private Vector2 curMovementInput;
    private PlayerClickMove _playerClickMove;
    private Rigidbody _rigidbody;

    [Header("Look")]
    public Transform target;
    public float camRotSpeed;
    private float camCurYRot;

    [Header("MoveRange")]
    public float maxRange;

    [HideInInspector]
    public bool canLook = true;

    private Vector2 lookPhase;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerClickMove = target.GetComponent<PlayerClickMove>();
    }

    private void FixedUpdate()
    {
        Move();
        CameraRotate();
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
        if (transform.position.x > target.position.x + maxRange)
        {
            transform.position = new Vector3(target.position.x + maxRange, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < target.position.x - maxRange)
        {
            transform.position = new Vector3(target.position.x - maxRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z > target.position.z + maxRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z + maxRange);
        }
        else if (transform.position.z < target.position.z - maxRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - maxRange);
        }

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= 10;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        if (!_playerClickMove.IsGrounded())
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
    }

    private void CameraRotate()
    {
        camCurYRot += lookPhase.x * camRotSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0, -camCurYRot, 0);
    }

    private void CameraLook()
    {
        transform.position = target.position;
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

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            lookPhase = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            lookPhase = Vector2.zero;
        }
    }

    public void OnFixationInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            canLook = !canLook;
        }
    }
}
