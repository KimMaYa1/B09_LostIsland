using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _craftItemTypeSlots;
    [SerializeField] private CraftedItemRecipe[] _recipe_Equipment;
    [SerializeField] private CraftedItemRecipe[] _recipe_Used;
    [SerializeField] private CraftedItemRecipe[] _recipe_Ingredient;
    [SerializeField] private CraftedItemRecipe[] _recipe_ETC;
    [SerializeField] private Image[] _buttonImages;

    private CraftSlot _craftSlot;
    private int _curTypeIndex = 0;
    private List<CraftedItemRecipe[]> _itemsList = new List<CraftedItemRecipe[]>();

    private void Start()
    {
        SetAllSlots();
    }

    private void SetAllSlots()
    {
        _itemsList.Add(_recipe_Equipment);
        _itemsList.Add(_recipe_Used);
        _itemsList.Add(_recipe_Ingredient);
        _itemsList.Add(_recipe_ETC);

        CraftSlot craftSlot;
        int childCount;
        for (int j = 0; j < _craftItemTypeSlots.Length; j++)
        {
            childCount = _craftItemTypeSlots[j].transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (i < _itemsList[j].Length)
                {
                    _craftItemTypeSlots[0].transform.GetChild(i).gameObject.SetActive(true);
                    craftSlot = _craftItemTypeSlots[0].transform.GetChild(i).GetComponent<CraftSlot>();
                    craftSlot.SetSlot(_itemsList[j][i].item.itemImage);
                }
            }
        }
    }

    private void UpdateCraftingUI(Item item)
    {
        //for (int i = 0; i < _curItems.Length; i++)
        //{
        //    _craftSlot = _curItems[i].GetComponent<CraftSlot>();
        //    if (i < _curItems.Length)
        //    {
        //        _curItems[i].SetActive(true);
        //        //_craftSlot.SetSlot(_curItems[i].itemImage);
        //    }
        //    else
        //    {
        //        _craftSlot.ClearSlot();
        //        _curItems[i].SetActive(false);
        //    }
        //}
    }

    public void SlotClick(int itemIndex)
    {
        UpdateCraftingUI(_itemsList[_curTypeIndex][itemIndex].item);
    }

    public void ButtonClick(int buttonIndex)
    {
        _craftItemTypeSlots[_curTypeIndex].SetActive(false);
        _craftItemTypeSlots[buttonIndex].SetActive(true);
        _curTypeIndex = buttonIndex;
        ResetButtonImages();
        _buttonImages[buttonIndex].color = Color.yellow;
    }

    private void ResetButtonImages()
    {
        for (int i = 0; i < _buttonImages.Length; i++)
        {
            _buttonImages[i].color = Color.white;
        }
    }

    private void ResetItemTypeSlots()
    {
        for (int i = 0; i < _craftItemTypeSlots.Length; i++)
        {
            _craftItemTypeSlots[i].SetActive(false);
        }
    }
}
