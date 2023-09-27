using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _craftItemSlots;
    [SerializeField] private CraftedItem[] _craftedItems;

    [SerializeField] private GameObject _craftingCanvas;
    [SerializeField] private GameObject _craftingCamera;

    private CraftSlot _craftSlot;
    private PlaceItemControll _placeItemControl;

    private void Start()
    {
        _placeItemControl = GetComponent<PlaceItemControll>();
        UpdateCraftingUI();
    }

    public void UpdateCraftingUI()
    {
        for(int i = 0; i <  _craftItemSlots.Length; i++)
        {
            _craftSlot = _craftItemSlots[i].GetComponent<CraftSlot>();
            if (i < _craftedItems.Length)
            {
                _craftItemSlots[i].SetActive(true);
                _craftSlot.SetSlot(_craftedItems[i]);
            }
            else
            {
                _craftSlot.ClearSlot();
                _craftItemSlots[i].SetActive(false);
            }
        }
    }

    public void SlotClick(int slotNumber)
    {
        _craftingCanvas.SetActive(false);
        _craftingCamera.SetActive(true);
        _placeItemControl.PreviewItemView(_craftedItems[slotNumber].itemPrefab);
    }

    

    private void Build()
    {
        
    }

}
