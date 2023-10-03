
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
    public Queue<GameObject> T_queue;//이부분 static으로 했었는데 그게 버그였음
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
        Vector3 randomDirection = Random.insideUnitSphere * 150f; // 원하는 범위 내의 랜덤한 방향 벡터를 생성합니다.
        randomDirection += transform.position; // 랜덤 방향 벡터를 현재 위치에 더합니다.

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 150f, NavMesh.AllAreas)) // 랜덤 위치가 NavMesh 위에 있는지 확인합니다.
        {
            return hit.position; // NavMesh 위의 랜덤 위치를 반환합니다.
        }
        else
        {
            return transform.position; // NavMesh 위의 랜덤 위치를 찾지 못한 경우 현재 위치를 반환합니다.
        }
    }

}
