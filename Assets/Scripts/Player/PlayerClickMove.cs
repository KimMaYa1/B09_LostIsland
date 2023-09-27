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
    public LayerMask groundLayerMask;
    private bool isItem;
    private bool isInteraction;
    private bool isJump;

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
        if (playerController.IsAttackDelay)
        {
            if (isMove)
            {
                Move();
            }
            Debug.Log(isJump);
            Debug.Log(isMove);
            if (_rigidbody.velocity.y == 0)
            {
                if (isJump)
                {
                    isMove = false;
                    isJump = false;
                }
            }
            

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);
        }
    }

    private void Move()
    {
        if (Vector3.Distance(destination, transform.position) <= 0.1f)
        {
            isMove = false;
            return;
        }

        Vector3 dir = direction.normalized;
        dir *= playerController.playerStat.MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        destination.y = transform.position.y;
        if (isItem || isInteraction)
        {
            isMove = (transform.position - destination).magnitude > 0.2f;
        }

        isMove = (transform.position - destination).magnitude > 0.05f;
    }

    public void OnClickMoveInput(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            if (context.phase == InputActionPhase.Canceled)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.collider.gameObject.layer != gameObject.layer)
                    {
                        isMove = true;
                        destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        direction = destination - transform.position;
                        if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
                        {
                            isItem = true;
                            isInteraction = false;
                        }
                        else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interaction"))
                        {
                            isInteraction = true;
                            isItem = false;
                        }
                    }
                    
                }
            }
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * playerController.playerStat.JumpForce, ForceMode.Impulse);
                isJump = true;
            }
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]        //�� �� �� �� ���ٰ� ray���� �׶���� �������ִ��� Ȯ��
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
}
