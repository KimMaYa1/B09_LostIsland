
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.UI;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public GameObject rangeObject;
    BoxCollider rangeCollider;
    public GameObject[] Model;
    public Queue<GameObject> T_queue;//이부분 static으로 했었는데 그게 버그였음
    public int count;

    [Serialize]private float spawnDleay=20;
    [Serialize] private float objectDleayCount;

    //public TreeSpawn Instance;

    private void Awake()
    {
        T_queue = new Queue<GameObject>();
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        //Instance= this;
    }

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject choiceModel = Model[Random.Range(0, Model.Count())];
            GameObject t_object = Instantiate(choiceModel, this.gameObject.transform);
            t_object.transform.Rotate(new Vector3(0, Random.Range(-180,180), 0));
            T_queue.Enqueue(t_object);
            t_object.SetActive(false);
        }

        StartCoroutine(TreeMaker());
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


    IEnumerator TreeMaker()
    {
        float dealy = 0.01f;
        while (true)
        {
            if (T_queue.Count != 0)
            {
                
                GameObject t_object = GetQueue();
                t_object.transform.position = Return_RandomPosition();
                if (T_queue.Count <= 5)
                    dealy = spawnDleay;

            }
            yield return new WaitForSeconds(dealy);
        }
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

}
