using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.SocialPlatforms;

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
    private float jumpDelay = 0;
    private float jumpDelayTime = 0;
    Vector3 startPos, endPos;
    LineRenderer lr;
    GameObject target;
    //test
    private NavMeshAgent nav;

    public InteractionTarget interactioncoll;

    private void Awake()
    {
        nav = GetComponentInChildren<NavMeshAgent>();
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
        if (jumpDelay > 0)
        {
            if (jumpDelayTime < jumpDelay)
            {
                jumpDelayTime += Time.deltaTime;
            }
            else
            {
                jumpDelayTime = 0;
                jumpDelay = 0;
            }
            Debug.Log(0);
        }
        if (UIManager.instance.inventoryActivated)
        {
            nav.ResetPath();
            nav.velocity = Vector3.zero;
        }
        Debug.Log(playerController.IsAttackDelay);
        if (playerController.IsAttackDelay)
        {
            if (isMove)
            {
                if (interactioncoll.target != null)
                {
                    if(target == interactioncoll.target)
                    {
                        _animator.SetBool("IsWalking", false);
                        nav.ResetPath();
                        nav.velocity = Vector3.zero;

                        Debug.Log("여긴 들어오냐");

                        if (isItem)
                        {
                            _animator.SetTrigger("Gather");
                            inventory.AcquireItem(target.GetComponent<ItemPickUp>().item);
                            Destroy(target);
                            isItem = false;
                        }
                        else if (isInteraction)
                        {
                            target.GetComponent<Door>().InteractionDoor();
                            isInteraction = false;
                        }
                        else if (isMonster)
                        {
                            Debug.Log("여기는");
                            transform.LookAt(target.transform.position);
                            playerController.OnAttackInput(inventory.currentWeapon);
                            
                            if (target.GetComponent<AnimalAI>().animalStats.States == State.Dead)
                            {
                                target = null;
                                interactioncoll.target = null;
                                isMonster = false;
                            }
                            else
                            {
                                return;
                            }
                            /*if ()
                            {
                                몬스터가 안죽었다면
                                0.5초 있다가 또 때리기
                                return;
                            }*/
                        }
                        isMove = false;
                    }
                }
                else if (target != null && ((1 << target.layer) | interactionManager.monsterLayerMask) == interactionManager.monsterLayerMask)
                {
                    if (nav.SetDestination(target.transform.position))
                    {
                        _animator.SetBool("IsWalking", true);
                    }
                }

                if (!nav.pathPending && nav.remainingDistance < 0.2f)
                {
                    _animator.SetBool("IsWalking", false);
                    nav.ResetPath();
                    nav.velocity = Vector3.zero;
                }
                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward), 0.25f);
                }
            }

            if (isJump)
            {
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
        if (Physics.Raycast(ray, out hit, 0.8f))
        {

        }
    }
    private IEnumerator Jump()
    {
        for (int i = 0; i < lr.positionCount - 1; i++)
        {
            Ray ray = new Ray(lr.GetPosition(i), lr.GetPosition(i + 1));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Vector3.Distance(lr.GetPosition(i), lr.GetPosition(i + 1)), jumpLayerMask))
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
        if (context.phase == InputActionPhase.Canceled )
        {
            if (!UIManager.instance.inventoryActivated)
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
                        target = null;
                        nav.velocity = Vector3.zero;


                        if (isJump)
                        {
                            isJump = false;
                            jumpDelay = 1;
                            _animator.SetBool("IsWalking", false);
                            StartCoroutine(Jump());
                            return;
                        }

                        isMove = true;

                        if (nav.SetDestination(hit.point))
                        {
                            _animator.SetBool("IsWalking", true);
                        }


                        if (((1 << hit.collider.gameObject.layer) | interactionManager.itemLayerMask) == interactionManager.itemLayerMask)
                        {
                            isItem = true;
                            target = hit.transform.gameObject;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.interactLayerMask) == interactionManager.interactLayerMask)
                        {
                            isInteraction = true;
                            target = hit.transform.gameObject;
                        }
                        else if (((1 << hit.collider.gameObject.layer) | interactionManager.monsterLayerMask) == interactionManager.monsterLayerMask)
                        {
                            isMonster = true;
                            target = hit.transform.gameObject;
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
                if (jumpDelay == 0)
                {
                    nav.velocity = Vector3.zero;
                    nav.ResetPath();
                    isJump = true;
                }
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
            if (Physics.Raycast(rays[i], 1.2f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}