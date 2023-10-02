using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public Item[] dropItem;
    public int quantityPerHit = 1;
    public int capacity;
    public GameObject spawnOBJ;
    private Spawner spawn;
    private void Start()
    {
        spawn=spawnOBJ.GetComponent<Spawner>();
    }


    //public void Gather(Vector3 hitPoint, Vector3 hitNormal) 
    public void Gather() 
    {
        
        for (int i = 0; i < quantityPerHit; ++i) 
        {
            Debug.Log("1차 포문");
            if (capacity <= 0) { break; }
            capacity -= 1;
            for (int j = 0; j < capacity; ++j) 
            {
                //Instantiate(dropItem[j].itemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
                Debug.Log("2차 포문");
                Instantiate(dropItem[j].itemPrefab, this.transform.position + Vector3.up, Quaternion.identity);
            }

        }
        if (capacity <= 0) 
        {

            spawn.InsertQueue(gameObject);
            this.gameObject.SetActive(false);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" && other.tag == "player")
        {
            Debug.Log("접촉 테스트 성공");
            Gather();
        }
    }
}
