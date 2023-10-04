using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("ItemType Button")]
    [SerializeField] private Image[] _buttonImages;

    [Header("ItemType Slot")]
    [SerializeField] private GameObject[] _craftItemTypeSlots;

    [Header("Item Recipe")]
    [SerializeField] private CraftedItemRecipe[] _recipe_Equipment;
    [SerializeField] private CraftedItemRecipe[] _recipe_Used;
    [SerializeField] private CraftedItemRecipe[] _recipe_Ingredient;
    [SerializeField] private CraftedItemRecipe[] _recipe_ETC;

    [Header("Craft UI")]
    [SerializeField] private GameObject _craftBG;
    [SerializeField] private Image _craftItemImage;
    [SerializeField] private TMP_Text _craftItemName;
    [SerializeField] private TMP_Text _craftItemInfo;
    [SerializeField] private GameObject[] _requiredItemSlots;
    [SerializeField] private Inventory _inventory;

    [Header("Slider")]
    [SerializeField] private Slider _slider;
    [SerializeField] TMP_Text _craftAmountTxt;
    private int _maxAmount = int.MaxValue;
    private int _craftAmount = 0;

    private int _curTypeIndex = 0;
    private List<CraftedItemRecipe[]> _itemsList = new List<CraftedItemRecipe[]>();
    private Item _curItem;
    private Item _lastReqItem;
    private string _curReqItemCountTxt;
    private CraftedItemRecipe _curRecipe;
    private Dictionary<Item, int> _requiredItemsDict = new Dictionary<Item, int>();

    private void Start()
    {
        SetAllSlots();
    }

    private void SetAllSlots()
    {
        _itemsList.Clear();
        _itemsList.Add(_recipe_Equipment);
        _itemsList.Add(_recipe_Used);
        _itemsList.Add(_recipe_Ingredient);
        _itemsList.Add(_recipe_ETC);

        CraftSlot craftSlot;
        int childCount;
        GameObject obj;
        for (int j = 0; j < _craftItemTypeSlots.Length; j++)
        {
            childCount = _craftItemTypeSlots[j].transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                obj = _craftItemTypeSlots[j].transform.GetChild(i).gameObject;
                if (i < _itemsList[j].Length)
                {
                    obj.SetActive(true);
                    craftSlot = obj.GetComponent<CraftSlot>();
                    craftSlot.SetSlot(_itemsList[j][i].item.itemImage);
                }
            }
        }

        _craftBG.SetActive(false);
    }

    public void SlotClick(int itemIndex)
    {
        _curRecipe = _itemsList[_curTypeIndex][itemIndex];
        UpdateReqItemDictFromInventory();
        if (_curRecipe != null)
            UpdateCraftingUI();
        _craftBG.SetActive(true);
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

    private void UpdateCraftingUI()
    {
        _curItem = _curRecipe.item;
        _craftItemName.text = _curItem.itemName;
        _craftItemInfo.text = _curItem.itemDesc;
        _craftItemImage.sprite = _curItem.itemImage;
        ReSetRequiredItemSlots();
        RequiredItemSlot requiredItemSlot;
        for (int i = 0; i < _curRecipe.requiredItems.Length; i++)
        {
            _requiredItemSlots[i].SetActive(true);
            requiredItemSlot = _requiredItemSlots[i].GetComponent<RequiredItemSlot>();

            _lastReqItem = _curRecipe.requiredItems[i];

            if (_requiredItemsDict.ContainsKey(_lastReqItem))
                _curReqItemCountTxt = _requiredItemsDict[(_lastReqItem)].ToString();
            else
                _curReqItemCountTxt = "0";

            requiredItemSlot.SetRequiredItemSlot(_lastReqItem.itemImage, _lastReqItem.itemName, _curReqItemCountTxt, _curRecipe.requiredItemsCount[i].ToString());

            if(_curRecipe.requiredItemsCount[i] != 0)
                CheckMaxAmount(int.Parse(_curReqItemCountTxt) / _curRecipe.requiredItemsCount[i]);
        }
        UpdateSlider();
    }

    private void ReSetRequiredItemSlots()
    {
        for (int i = 0; i < _requiredItemSlots.Length; i++)
        {
            _requiredItemSlots[i].SetActive(false);
        }
    }

    private void CheckMaxAmount(int amount)
    {
        _maxAmount = _maxAmount > amount ? amount : _maxAmount;
    }

    private void UpdateSlider()
    {
        _slider.maxValue = _maxAmount;
        _craftAmountTxt.text = ((int)_slider.value).ToString();
    }

    public void SetCraftAmount()
    {
        _craftAmount = (int)_slider.value;
        _craftAmountTxt.text = _craftAmount.ToString();
    }

    public void MinusSliderValue()
    {
        _slider.value -= 1;
    }

    public void PlusSliderValue()
    {
        _slider.value += 1;
    }

    public void MinSliderValue()
    {
        _slider.value = 0;
    }

    public void MaxSliderValue()
    {
        _slider.value = _maxAmount;
    }

    public void OnCraft()
    {
        int changeAmount = 0;
        if (_craftAmount > 0)
        {
            _inventory.AcquireItem(_curRecipe.item, _curRecipe.itemCount * _craftAmount);
            for(int i = 0; i < _curRecipe.requiredItems.Length; i++)
            {
                changeAmount = -(_curRecipe.requiredItemsCount[i] * _craftAmount);
                _inventory.DeAcquireItem(_curRecipe.requiredItems[i], changeAmount);
                _requiredItemsDict[_curRecipe.requiredItems[i]] += changeAmount;
            }
        }

        UpdateCraftingUI();
    }

    private void UpdateReqItemDictFromInventory()
    {
        Slot[] slots = _inventory.GetSlots();
        Item curItem;
        int curItemCount = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                curItem = slots[i].item;
                curItemCount = slots[i].itemCount;
                if (_requiredItemsDict.Count > 0 && _requiredItemsDict.ContainsKey(curItem))
                    _requiredItemsDict[curItem] = curItemCount;
                else
                    _requiredItemsDict.Add(curItem, curItemCount);
            }
        }
    }
}
