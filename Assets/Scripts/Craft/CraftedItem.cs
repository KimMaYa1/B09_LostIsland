using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Crafted Item")]
public class CraftedItem : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string itemDesc;

    public Sprite itemSprite;
    public GameObject itemPrefab;

    public Item[] requiredItems;
}