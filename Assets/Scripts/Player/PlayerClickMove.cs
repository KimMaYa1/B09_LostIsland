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
    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool isMove;
    public LayerMask groundLayerMask;
    public LayerMask jumpLayerMask;
    public Inventory inventory;
    private bool isItem;
    private bool isInteraction;
    private bool isMonster;
    private bool isJump = false;
    Vector3 startPos, endPos;
    LineRenderer lr;
    GameObject target;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
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
        if (playerController.IsAttackDelay) //TODO&& !UIManager.instance.inventoryActivated)
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

                        destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        direction = destination - transform.position;

                        if (transform.forward != direction.normalized)
                        {
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.25f);
                        }

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
                _animator.SetBool("IsWalking", false);
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

    private void Attack()
    {
        Ray ray = new Ray(transform.position + (transform.forward * 0.15f) + (-transform.up * 0.5f), Vector3.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 0.8f))
        {

        }
    }

    private void Move()
    {
        _animator.SetBool("IsWalking", true);
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
        if (isItem || isInteraction)
        {
            a = 0.8f;
        }
        else
        {
            a = 0.2f;
        }

        isMove = (transform.position - destination).magnitude > a;
        if(!isMove && isItem)
        {
            inventory.AcquireItem(target.GetComponent<ItemPickUp>().item);
            Destroy(target.transform.gameObject);
        }
        else if(!isMove && isInteraction)
        {
            target.GetComponent<Door>().InteractionDoor();
        }
    }

    private IEnumerator Jump()
    {
        for(int i = 0; i < lr.positionCount-1; i++)
        {
            Ray ray = new Ray(lr.GetPosition(i), lr.GetPosition(i+1));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i + 1)), jumpLayerMask))
            {
                yield break;
            }
        }
        _animator.SetTrigger("Jump");
        for (int i = 0; i < lr.positionCount; i++)
        {
            Vector3 a = lr.GetPosition(i);
            transform.position = new Vector3(a.x, a.y + 0.95f, a.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield break;
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
                        isItem = false;
                        isInteraction = false;
                        isMonster = false;

                        destination = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                        direction = destination - transform.position;

                        if (isJump)
                        {
                            isJump = false;
                            StartCoroutine(Jump());
                            return;
                        }
                        isMove = true;

                        if (((1 << hit.collider.gameObject.layer) | interactionManager.itemLayerMask) == interactionManager.itemLayerMask)
                        {
                            isItem = true;
                            target = hit.transform.gameObject;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.interactLayerMask) == interactionManager.interactLayerMask)
                        {
                            isInteraction = true;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.monsterLayerMask) == interactionManager.monsterLayerMask)
                        {
                            isMonster = true;
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

    public bool IsGrounded()
    {
        Ray[] rays = new Ray[8]        //앞 뒤 왼 오 에다가 ray만들어서 그라운드와 만나고있는지 확인
        {
            new Ray(transform.position + (transform.forward * 0.15f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.15f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.3f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.3f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.3f) + (transform.forward * 0.15f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.3f) + (-transform.forward * 0.15f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.3f) + (transform.forward * 0.15f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.3f) + (-transform.forward * 0.15f), Vector3.down),
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