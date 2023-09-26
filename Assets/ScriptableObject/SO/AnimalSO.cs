using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAnimalData", menuName = "Animals/Default", order = 0)]
public class AnimalSO : ScriptableObject
{
    [Header("Animal Info")]
    public bool isAttack;           // 공격하는 동물인지
    public float moveSpeed;         // 걸을 때 스피드
    public float runSpeed;          // 뛸 때 스피드
    public float power;             // 파워
    public float health;            // 최대 체력
    public float currentHealth;     // 체력
    public float range;             // 어그로 거리
    public float attackRange;       // 어택 거리
}
