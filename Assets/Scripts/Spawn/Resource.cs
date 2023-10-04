using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public Item[] dropItem;
    public Item[] RareDropItem;
    //public int quantityPerHit = 1;
    public int capacity=2;
    public GameObject spawnOBJ;
    private Spawner spawn;
    private void Start()
    {
        spawn=spawnOBJ.GetComponent<Spawner>();
    }


    //public void Gather(Vector3 hitPoint, Vector3 hitNormal) 
    public void Gather() 
    {
        capacity -= 1;
        if (capacity <= 0) 
        {
            for (int j = 0; j < dropItem.Length; ++j)
            {
                //Instantiate(dropItem[j].itemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
                //Debug.Log("2차 포문");
                Instantiate(dropItem[j].itemPrefab, this.transform.position + Vector3.up, Quaternion.identity);
            }
            int random = Random.Range(0, 100);
            if (random <= 20 && RareDropItem[0] != null) 
            {
                for (int k = 0; k < dropItem.Length; ++k)
                {
                    Instantiate(RareDropItem[k].itemPrefab, this.transform.position + Vector3.up, Quaternion.identity);
                }
            }
            spawn.InsertQueue(gameObject);
            this.gameObject.SetActive(false);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "player")
        {
            //Debug.Log("접촉 테스트 성공");
            Gather();
        }
    }
}
