using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public interface IState
{
    //���� ����
    void Enter();

    //���� ����
    void Stay();

    //���� Ż��
    void Exit();

}

public class AnimalAI : MonoBehaviour
{
    // �ִϸ�����
    public Animator animator;
    // ���� ����
    public AnimalStats animalStats;
    // ���� ���� �ӽ�
    public IState[] _IStates;
    [SerializeField]
    private State _state;
    // navMesh
    public NavMeshAgent nav;

    void Awake()
    {
        _IStates = new IState[System.Enum.GetValues(typeof(State)).Length];
        _IStates[(int)State.Idle] = new IdleState(this, animalStats);
        _IStates[(int)State.Walk] = new WalkState(this, animalStats);
        _IStates[(int)State.Run] = new RunState(this, animalStats);
        _IStates[(int)State.Attack] = new AttackState(this, animalStats);
        _IStates[(int)State.Dead] = new DeadState(this, animalStats);
        _IStates[(int)State.GetHit] = new GetHitState(this, animalStats);
        _IStates[(int)State.Chase] = new ChaseState(this, animalStats);
        States = State.Idle;
    }

    private void Update()
    {
        _IStates[(int)_state].Stay();
        if (IsDeadCheck()) {
            _state = State.Dead;
        }
    }

    public enum State
    {
        Idle,
        Walk,
        Run,
        Attack,
        GetHit,
        Dead,
        Chase
    }

    public State States
    {
        get { return _state; }
        set
        {
            _IStates[(int)_state].Exit();
            _state = value;
            _IStates[(int)_state].Enter();
        }
    }

    // ������ ��ġ ��ȯ
    public Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * animalStats.animalSO.range; // ���ϴ� ���� ���� ������ ���� ���͸� �����մϴ�.
        randomDirection += transform.position; // ���� ���� ���͸� ���� ��ġ�� ���մϴ�.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas)) // ���� ��ġ�� NavMesh ���� �ִ��� Ȯ���մϴ�.
        {
            return hit.position; // NavMesh ���� ���� ��ġ�� ��ȯ�մϴ�.
        }
        else
        {
            return transform.position; // NavMesh ���� ���� ��ġ�� ã�� ���� ��� ���� ��ġ�� ��ȯ�մϴ�.
        }
    }

    //���� ó��
    public void DeadAnimal()
    {
        Destroy(this.gameObject);
    }

    //���� �� üũ
    public bool IsDeadCheck()
    {
        if(animalStats.currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayAnimation(State state)
    {
        switch (state)
        {
            case State.Idle:
                animator.Play("Idle");
                break;
            case State.GetHit:
                animator.Play("GetHit");
                break;
            case State.Attack:
                if(animalStats.animal == Animal.Bear)
                {
                    animator.Play("Attack" + Random.Range(1, 5));
                }
                else if(animalStats.animal == Animal.Fox)
                {
                    animator.Play("Attack" + Random.Range(1, 2));
                }
                else
                {
                    animator.Play("Attack");
                }
                break;
            case State.Run:
                animator.Play("Run");
                break;
            case State.Dead:
                animator.Play("Dead");
                break;
            case State.Walk:
                animator.Play("Walk");
                break;
        }
    }
}
