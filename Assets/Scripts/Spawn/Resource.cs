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
    public void Gather(Vector3 hitPoint, Vector3 hitNormal) 
    {
        for (int i = 0; i < quantityPerHit; ++i) 
        {
            if (capacity <= 0) { break; }
            capacity -= 1;
            for (int j = 0; j < capacity; ++j) 
            {
                Instantiate(dropItem[j].itemPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }

        }
        if (capacity <= 0) 
        {

            spawn.InsertQueue(gameObject);
            Destroy(gameObject);

        }
    }
}