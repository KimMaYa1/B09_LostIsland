using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClickMove : MonoBehaviour
{
    private Camera camera;
    private Vector3 destination;
    private Vector3 direction;
    private PlayerController playerController;
    private Rigidbody _rigidbody;
    private bool isMove;

    private void Awake()
    {

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        camera = Camera.main;
    }

    private void Update()
    {
        if (isMove)
        {
            Move();
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);
    }

    private void Move()
    {
        if (Vector3.Distance(destination, transform.position) <= 0.5f)
        {
            isMove = false;
            return;
        }
        /*Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= playerStat.MoveSpeed;*/

        Vector3 dir = direction.normalized;
        dir *= playerController.playerStat.MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        isMove = (transform.position - destination).magnitude > 1f;
    }

    public void OnClickMoveInput(InputAction.CallbackContext context)
    {
        if (_rigidbody.velocity.y == 0)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (_rigidbody.velocity.y != 0 && _rigidbody.velocity.x == 0 && _rigidbody.velocity.z == 0)
                {
                    isMove = false;
                    return;
                }

                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    isMove = true;
                    destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    direction = destination - transform.position;
                }
            }
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
                _rigidbody.AddForce(Vector2.up * playerController.playerStat.JumpForce, ForceMode.Impulse);
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
            if (Physics.Raycast(rays[i], 1f, playerController.groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
