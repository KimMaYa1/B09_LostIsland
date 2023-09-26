using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
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
    private State _state;
    //���� ������ �ٵ�
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

    //���� ó��
    public void DeadAnimal()
    {
        Destroy(this.gameObject);
    }

    //���� �� üũ
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
