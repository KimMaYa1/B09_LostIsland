using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;

public class PlaceItemControll : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _mainCamera;
    
    [SerializeField] private Transform _placeTransform;
    [SerializeField] private Transform _playerTransform;

    private GameObject _craftedItemPrefab;

    [SerializeField] private LayerMask layerMask;
    private RaycastHit hit;
    private float range = 10f;

    private Vector3 _originPlace;
    private bool isPrefabActivated = false;
    private bool isItemMoving = false;
    private Vector3 curMovementInput;
    private Coroutine _coroutine;
    private float _moveSpeed = 2f;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _originPlace = _camera.transform.position;
    }

    void Update()
    {
        CraftedCameraUpdate();

        if (isPrefabActivated)
            PrefabPositionUpdate();
    }

    private void CraftedCameraUpdate()
    {
        _camera.transform.position = _originPlace + _placeTransform.position;

        float x = _camera.transform.rotation.eulerAngles.x;
        float y = _placeTransform.rotation.eulerAngles.y;

        _camera.transform.localRotation = Quaternion.Euler(x, y, 0);
    }

    public void PreviewItemView(GameObject itemPrefab)
    {
        _craftedItemPrefab = Instantiate(itemPrefab, _playerTransform.position + _playerTransform.forward, Quaternion.identity);
        _rigidbody = _craftedItemPrefab.GetComponent<Rigidbody>();
        isPrefabActivated = true;
    }

    private void PrefabPositionUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            if (hit.transform != null)
            {
                Vector3 _location = hit.point;
                _craftedItemPrefab.transform.position = _location;
            }
        }
    }

    public void OnMoveItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Performed 己傍");
            isItemMoving = true;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(MoveItemCo());
            curMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isItemMoving = false;
            StopCoroutine(_coroutine);
            curMovementInput = Vector2.zero;
        }
    }

    IEnumerator MoveItemCo()
    {
        Transform itemTransform = _craftedItemPrefab.transform;
        while (isItemMoving)
        {
            Debug.Log("内风凭 己傍");

            Vector3 dir = itemTransform.forward * curMovementInput.y + itemTransform.right * curMovementInput.x + itemTransform.up * curMovementInput.z;
            dir *= _moveSpeed;

            _rigidbody.velocity = dir;
        }
        yield return null;
    }
}
