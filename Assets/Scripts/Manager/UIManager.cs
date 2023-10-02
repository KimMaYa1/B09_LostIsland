using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject _uiPrefab;
    [SerializeField] private GameObject _uiInventory;
    [SerializeField] private GameObject _uiCrafting;
    [SerializeField] private GameObject _uiItemInteract;
    private GameObject _uiObject;
    private Image _uiItemInteractBGImage;
    private float _originValue;
    private float _interactX;
    private float _interactY;
    private float _interactWidth;
    private float _interactHeight;

    TMP_Text itemText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _uiItemInteract.SetActive(true);
        itemText = _uiItemInteract.GetComponentInChildren<TMP_Text>();
        _uiItemInteractBGImage = _uiItemInteract.GetComponent<Image>();
        _originValue = _uiItemInteractBGImage.color.a;
        ClearInteractItem();
        _interactWidth = _uiItemInteract.GetComponent<RectTransform>().rect.width;
        _interactHeight = _uiItemInteract.GetComponent<RectTransform>().rect.height;
    }

    public void TabInventory()
    {
        if (_uiInventory.activeSelf)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            _uiInventory.SetActive(false);
        }
        else
        {
            //Cursor.lockState = CursorLockMode.Confined;
            _uiInventory.SetActive(true);
        }
    }

    public void TabCraftingUI()
    {
        if (_uiCrafting.activeSelf)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            _uiCrafting.SetActive(false);
        }
        else
        {
            //Cursor.lockState = CursorLockMode.Confined;
            _uiCrafting.SetActive(true);
        }
    }

    public void InteractItem(Item item)
    {
        float x = _interactWidth * Screen.width / 1920f;
        float y = _interactHeight * Screen.height / 1080f;

        _interactX = Input.mousePosition.x;
        _interactY = Input.mousePosition.y;
        _interactX = _interactX > Screen.width / 2 ? _interactX - x / 2 : _interactX + x / 2;
        _interactY = _interactY > Screen.height / 2 ? _interactY - y / 2 : _interactY + y / 2;
        _uiItemInteract.transform.position = new Vector2(_interactX, _interactY);

        Color color = _uiItemInteractBGImage.color;
        _uiItemInteractBGImage.color = new Color(color.r, color.g, color.b, _originValue);
        itemText.text = string.Format($"<b>[F]</b> {item.Interactable()}");
    }

    public void ClearInteractItem()
    {
        _uiItemInteractBGImage.color = Color.clear;
        itemText.text = string.Empty;
    }
}
