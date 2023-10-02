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
    Vector3 startPos, endPos;
    LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
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
            if (isJump)
            {
                isMove = false;
                lr.enabled = true;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.collider.gameObject.layer != gameObject.layer)
                    {
                        startPos = new Vector3(transform.position.x, transform.position.y - 0.95f, transform.position.z);
                        endPos = hit.point;

                        Vector3 center = (startPos + endPos) * 0.5f;

                        center.y -= 3;

                        startPos = startPos - center;
                        endPos = endPos - center;

                        for (int i = 0; i < lr.positionCount; i++)
                        {
                            Vector3 point = Vector3.Slerp(startPos, endPos, i / (float)(lr.positionCount - 1));
                            point += center;

                            lr.SetPosition(i, point);
                        }
                    }
                }
            }
            else
            {
                lr.enabled = false;
            }

            if (isMove)
            {
                Move();
            }
            else
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                isItem = false;
                isInteraction = false;
                isMonster = false;
            }

            /*if (_rigidbody.velocity.y == 0)
            {
                if (isJump)
                {
                    _rigidbody.velocity = Vector3.zero;
                    isMove = false;
                    isJump = false;
                    isItem = false;
                    isInteraction = false;
                    isMonster = false;
                }
            }
            if (_rigidbody.velocity.y > 0)
            {
                isJump = true;
            }*/
        }
    }

    private void Move()
    {
        if (Vector3.Distance(destination, transform.position) <= 0.1f)
        {
            isMove = false;
            return;
        }

        if (transform.forward != direction.normalized)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);
        }

        Vector3 dir = direction.normalized;
        dir *= playerController.playerStat.MoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        destination.y = transform.position.y;

        float a = 0;
        if (isItem || isInteraction || isMonster)
        {
            a = 0.8f;
        }
        else
        {
            a = 0.2f;
        }

        isMove = (transform.position - destination).magnitude > a;
    }

    private IEnumerator Jump()
    {
        for (int i = 0; i < lr.positionCount; i++)
        {
            Vector3 a = lr.GetPosition(i);
            transform.position = new Vector3(a.x, a.y + 0.95f, a.z);
            yield return new WaitForSeconds(Time.deltaTime*2.5f);
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
                        destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        direction = destination - transform.position;

                        if (isJump)
                        {
                            transform.rotation = Quaternion.LookRotation(direction);
                            StartCoroutine(Jump());
                            isJump = false;
                            return;
                        }
                        isMove = true;

                        if (((1 << hit.collider.gameObject.layer) | interactionManager.itemLayerMask) == interactionManager.itemLayerMask)
                        {
                            isItem = true;
                            isInteraction = false;
                            isMonster = false;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.interactLayerMask) == interactionManager.interactLayerMask)
                        {
                            isInteraction = true;
                            isItem = false;
                            isMonster = false;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.monsterLayerMask) == interactionManager.monsterLayerMask)
                        {
                            isMonster = true;
                            isItem = false;
                            isInteraction = false;
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
                /*_rigidbody.AddForce(Vector2.up * playerController.playerStat.JumpForce, ForceMode.Impulse);*/
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
            if (Physics.Raycast(rays[i], 1.5f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}