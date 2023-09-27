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
    private InteractionManager interactionManager;
    private Rigidbody _rigidbody;
    private bool isMove;
    public LayerMask groundLayerMask;
    private bool isItem;
    private bool isInteraction;
    private bool isMonster;
    private bool isJump = false;

    private void Awake()
    {

        _rigidbody = GetComponent<Rigidbody>();
        interactionManager = GetComponent<InteractionManager>();
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
            else if (!isJump)
            {
                _rigidbody.velocity = Vector3.zero;
                isItem = false;
                isInteraction = false;
            }

            if (IsGrounded())
            {
                if (isJump)
                {
                    isMove = false;
                    isJump = false;
                }
            }

            //Debug.Log(isItem);
            //Debug.Log(isInteraction);

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
            isMove = (transform.position - destination).magnitude > 0.5f;
        }
        else
        {
            isMove = (transform.position - destination).magnitude > 0.05f;
        }
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

                        if(((1<<hit.collider.gameObject.layer)| interactionManager.itemLayerMask) == interactionManager.itemLayerMask)
                        {
                            isItem = true;
                            isInteraction = false;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.interactLayerMask) == interactionManager.interactLayerMask)
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
}
