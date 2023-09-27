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
    //상태 진입
    void Enter();

    //상태 유지
    void Stay();

    //상태 탈출
    void Exit();

}

public class AnimalAI : MonoBehaviour
{
    // 애니메이터
    public Animator animator;
    // 동물 정보
    public AnimalStats animalStats;
    // 유한 상태 머신
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

    // 랜덤한 위치 반환
    public Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * animalStats.animalSO.range; // 원하는 범위 내의 랜덤한 방향 벡터를 생성합니다.
        randomDirection += transform.position; // 랜덤 방향 벡터를 현재 위치에 더합니다.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas)) // 랜덤 위치가 NavMesh 위에 있는지 확인합니다.
        {
            return hit.position; // NavMesh 위의 랜덤 위치를 반환합니다.
        }
        else
        {
            return transform.position; // NavMesh 위의 랜덤 위치를 찾지 못한 경우 현재 위치를 반환합니다.
        }
    }

    //죽음 처리
    public void DeadAnimal()
    {
        Destroy(this.gameObject);
    }

    //죽은 지 체크
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
