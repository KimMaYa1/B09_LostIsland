
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.AI;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject rangeObject;
    public GameObject[] Model;
    public int[] modelCount = { 20, 15, 7, 7, 5 };
    public Queue<GameObject> T_queue;//�̺κ� static���� �߾��µ� �װ� ���׿���
    public int count;
    public NavMeshAgent nav;
    public static AnimalSpawner Instance;

    private void Awake()
    {
        T_queue = new Queue<GameObject>();
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            for(int j = 0; j < modelCount[i]; j++)
            {
                GameObject choiceModel = Model[Random.Range(0, Model.Count())];
                GameObject t_object = Instantiate(choiceModel);
                T_queue.Enqueue(t_object);
                t_object.SetActive(false);
            }
        }

        StartCoroutine(AnimalMaker());
    }

    public void InsertQueue(GameObject p_object)
    {
        T_queue.Enqueue(p_object);
        p_object.SetActive(false);
    }
    public Queue<GameObject> returnQueue() 
    {
        return T_queue;
    }

    public GameObject GetQueue()
    {
        GameObject t_object = T_queue.Dequeue();
        t_object.SetActive(true);

        return t_object;
    }


    IEnumerator AnimalMaker()
    {
        while (true)
        {
            if (T_queue.Count != 0)
            {
                GameObject t_object = GetQueue();
                t_object.GetComponent<AnimalAI>().nav.enabled = false;
                t_object.transform.position = GetRandomPositionOnNavMesh();
                t_object.GetComponent<AnimalAI>().nav.enabled = true;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public Vector3 GetRandomPositionOnNavMesh()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 150f; // ���ϴ� ���� ���� ������ ���� ���͸� �����մϴ�.
        randomDirection += transform.position; // ���� ���� ���͸� ���� ��ġ�� ���մϴ�.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 150f, NavMesh.AllAreas)) // ���� ��ġ�� NavMesh ���� �ִ��� Ȯ���մϴ�.
        {
            return hit.position; // NavMesh ���� ���� ��ġ�� ��ȯ�մϴ�.
        }
        else
        {
            return transform.position; // NavMesh ���� ���� ��ġ�� ã�� ���� ��� ���� ��ġ�� ��ȯ�մϴ�.
        }
    }

}
