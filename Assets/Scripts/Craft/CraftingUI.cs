using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _craftItemSlots;
    [SerializeField] private Item[] _craftedItems;


    private CraftSlot _craftSlot;

    private void Start()
    {
        UpdateCraftingUI();
    }

    public void UpdateCraftingUI()
    {
        for (int i = 0; i < _craftItemSlots.Length; i++)
        {
            _craftSlot = _craftItemSlots[i].GetComponent<CraftSlot>();
            if (i < _craftedItems.Length)
            {
                _craftItemSlots[i].SetActive(true);
                _craftSlot.SetSlot(_craftedItems[i], i);
            }
            else
            {
                _craftSlot.ClearSlot();
                _craftItemSlots[i].SetActive(false);
            }
        }
    }

    

}
