using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private Item _crafteditem;
    
    public void SetSlot(Item craftedItem)
    {
        _crafteditem = craftedItem;
        _icon.sprite = _crafteditem.itemImage;
    }

    public void ClearSlot()
    {
        _crafteditem = null;
        _icon.sprite = null;
    }
}
