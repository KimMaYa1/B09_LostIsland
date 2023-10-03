using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;
    private Button _button;
    private Item _crafteditem;
    private int _itemIndex;

    private void Awake()
    {
        _itemIndex = -1;
        _button = GetComponent<Button>();
    }

    public void SetSlot(Item craftedItem, int itemIndex)
    {
        _itemIndex = itemIndex;
        _crafteditem = craftedItem;
        _icon.sprite = _crafteditem.itemImage;
    }

    public void ClearSlot()
    {
        _itemIndex = -1;
        _crafteditem = null;
        _icon.sprite = null;
    }

    public int GetItemIndex()
    {
        return _itemIndex;
    }
}
