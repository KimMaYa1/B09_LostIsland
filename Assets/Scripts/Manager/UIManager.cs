using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject _uiPrefab;
    [SerializeField] private GameObject _uiInventory;
    [SerializeField] private GameObject _uiItemInteract;
    [SerializeField] private GameObject _inventoryButton;
    [SerializeField] private GameObject _craftButton;
    private Image _uiItemInteractBGImage;
    private float _originValue;
    private float _interactX;
    private float _interactY;
    private float _interactWidth;
    private float _interactHeight;

    [SerializeField] private PlaceItemController _placeItemController;
    [SerializeField] private Inventory _inventory;

    private Item _previewItem;
    private bool _isPreviewOn = false;

    TMP_Text itemText;


    public bool inventoryActivated = false;

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
        inventoryActivated = !inventoryActivated;
        if (_isPreviewOn)
        {
            _placeItemController.ClearPreview();
            _isPreviewOn = false;
        }
        SetInventoryObjects(!_uiInventory.activeSelf);
    }

    private void SetInventoryObjects(bool isActive)
    {
        _uiInventory.SetActive(isActive);
        _inventoryButton.SetActive(isActive);
        _craftButton.SetActive(isActive);
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
        itemText.text = string.Format($"{item.Interactable()}");
    }

    public void ClearInteractItem()
    {
        _uiItemInteractBGImage.color = Color.clear;
        itemText.text = string.Empty;
    }

    public void SlotClickTypeETC(Item item)
    {
        _previewItem = item;
        TabInventory();
        _placeItemController.PreviewItemView(item.itemPrefab);
        _isPreviewOn = true;

    }

    public void OnBuild(InputAction.CallbackContext context)
    {
        if (_isPreviewOn == true)
            if (context.phase == InputActionPhase.Started)
            {
                _placeItemController.PlacePrefab();
                _inventory.DeAcquireItem(_previewItem);
                _previewItem = null;
                _isPreviewOn = false;
            }
    }
}
