using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultAnimalData", menuName = "Animals/Default", order = 0)]
public class AnimalSO : ScriptableObject
{
    [Header("Animal Info")]
    public bool isAttack;           // �����ϴ� ��������
    public float moveSpeed;         // ���� �� ���ǵ�
    public float runSpeed;          // �� �� ���ǵ�
    public float power;             // �Ŀ�
    public float health;            // �ִ� ü��
    public float currentHealth;     // ü��
    public float range;             // ��׷� �Ÿ�
    public float attackRange;       // ���� �Ÿ�
}
