using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
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
    private State _state;
    //동물 리지드 바디
    public Rigidbody animalRigidbody;

    void Awake()
    {
        _IStates = new IState[System.Enum.GetValues(typeof(State)).Length];
        _IStates[(int)State.Idle] = new IdleState(this, animalStats);
        _IStates[(int)State.Walk] = new WalkState(this, animalStats);
        _IStates[(int)State.Run] = new RunState(this, animalStats);
        _IStates[(int)State.Attack] = new AttackState(this, animalStats);
        _IStates[(int)State.Death] = new DeadState(this, animalStats);
        _IStates[(int)State.GetHit] = new GetHitState(this, animalStats);
    }

    private void Update()
    {
        _IStates[(int)_state].Stay();
    }

    public enum State
    {
        Idle,
        Walk,
        Run,
        Attack,
        GetHit,
        Death
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

    //죽음 처리
    public void DeadAnimal()
    {
        Destroy(this.gameObject);
    }

    //죽은 지 체크
    public bool IsDeadCheck(AnimalStats animalStats)
    {
        if(animalStats.animalSO.currentHealth <= 0)
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
                animator.Play("Attack");
                break;
            case State.Run:
                animator.Play("Run");
                break;
            case State.Death:
                animator.Play("Death");
                break;
            case State.Walk:
                animator.Play("Walk");
                break;
        }
    }
}
