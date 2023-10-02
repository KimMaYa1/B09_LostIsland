using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Crafted Item Recipe")]
public class CraftedItemRecipe : ScriptableObject
{
    public string recipeName;
    [TextArea]
    public string recipeDesc;

    public Item item;
    public int itemCount;
    public Item[] requiredItems;
    public int[] requiredItemsCount;
}