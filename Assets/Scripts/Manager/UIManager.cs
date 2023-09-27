using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject _uiPrefab;
    [SerializeField] private GameObject _uiInventory;
    [SerializeField] private GameObject _uiCrafting;
    private GameObject _uiObject;

    private void Awake()
    {
        instance = this;
    }

    public void TabInventory()
    {
        if (_uiInventory.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _uiInventory.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            _uiInventory.SetActive(true);
        }
    }

    public void TabCraftingUI()
    {
        if (_uiCrafting.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _uiCrafting.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            _uiCrafting.SetActive(true);
        }
    }
}
