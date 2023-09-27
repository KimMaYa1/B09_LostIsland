using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _craftItemSlots;
    [SerializeField] private CraftedItem[] _craftedItems;

    [SerializeField] private GameObject _craftingCanvas;
    [SerializeField] private GameObject _craftingCamera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Camera _camera;

    private CraftSlot _craftSlot;
    private GameObject _craftedItemPrefab;
    //private GameObject _craftedItemPreview;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range = 10f;

    private bool isActivated = false;
    private bool isPreviewActivated = false;

    
    private void Start()
    {
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
        //_craftedItemPreview = Instantiate(_craftedItems[slotNumber].itemPrefab, _playerTransform.position + _playerTransform.forward, Quaternion.identity);
        //_craftedItemPreview.GetComponent<PreviewObject>().gameObject.SetActive(true);
        //_craftedItemPrefab = _craftedItems[slotNumber].itemPrefab;
        _craftingCamera.SetActive(true);
        _craftedItemPrefab = Instantiate(_craftedItems[slotNumber].itemPrefab, _playerTransform.position + _playerTransform.forward, Quaternion.identity);
        isPreviewActivated = true;
        _craftingCanvas.SetActive(false);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        //    Window();

        if (isPreviewActivated)
            PreviewPositionUpdate();

        //if (Input.GetButtonDown("Fire1"))
        //    Build();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Cancel();
    }

    private void PreviewPositionUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            if (hit.transform != null)
            {
                Vector3 _location = hit.point;
                _craftedItemPrefab.transform.position = _location;

                Debug.Log("로케이션 : " + _location);
                Debug.Log("프리팹 위치 : " + _craftedItemPrefab.transform.position);
            }
        }

        //if (Physics.Raycast(_playerTransform.position, _playerTransform.forward, out hit, range, layerMask))
        //{
        //    if (hit.transform != null)
        //    {
        //        Vector3 _location = hit.point;
        //        _craftedItemPrefab.transform.position = _location;

        //        Debug.Log("로케이션 : " + _location);
        //        Debug.Log("프리팹 위치 : " + _craftedItemPrefab.transform.position);
        //    }
        //}
    }

    private void Build()
    {
        //if (isPreviewActivated && _craftedItemPreview.GetComponent<PreviewObject>().isBuildable())
        //{
        //    Instantiate(_craftedItemPrefab, hitInfo.point, Quaternion.identity);
        //    Destroy(_craftedItemPreview);
        //    isActivated = false;
        //    isPreviewActivated = false;
        //    _craftedItemPreview = null;
        //    _craftedItemPrefab = null;
        //}
    }

    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        _craftingCanvas.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        _craftingCanvas.SetActive(false);
    }

    private void Cancel()
    {
        //if (isPreviewActivated)
        //    Destroy(_craftedItemPreview);

        //isActivated = false;
        //isPreviewActivated = false;

        //_craftedItemPreview = null;
        //_craftedItemPrefab = null;

        //_craftingCanvas.SetActive(false);
    }
}
