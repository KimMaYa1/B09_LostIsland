using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private CraftedItem _crafteditem;
    
    public void SetSlot(CraftedItem craftedItem)
    {
        _crafteditem = craftedItem;
        _icon.sprite = _crafteditem.itemSprite;
    }

    public void ClearSlot()
    {
        _crafteditem = null;
        _icon.sprite = null;
    }
}
