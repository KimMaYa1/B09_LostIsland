using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceItemController : MonoBehaviour
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
    private Vector3 _curMovementInput;
    private Vector3 _prefabForword;
    private Vector3 _prefabRight;
    private Vector3 _prefabUp;
    private bool _isPrefabActivated = false;
    private bool _isItemMoving = false;
    private float _moveSpeed = 2f;
    private Coroutine _coroutine;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private List<Material> _materials = new List<Material>();
    private List<Color> _originColors = new List<Color>();

    private void Start()
    {
        _originPlace = _camera.transform.position;
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
        CraftedCameraUpdate();
        if (_craftedItemPrefab == null)
            _craftedItemPrefab = Instantiate(itemPrefab, _playerTransform.position + _playerTransform.forward, Quaternion.identity);

        _meshRenderer = _craftedItemPrefab.GetComponent<MeshRenderer>();
        _materials.Clear();
        _materials = _meshRenderer.materials.ToList();
        _originColors.Clear();
        for (int i = 0; i < _materials.Count; i++)
        {
            _originColors.Add(_materials[i].color);
        }
        SetColor(Color.green);

        _rigidbody = _craftedItemPrefab.GetComponent<Rigidbody>();
        _isPrefabActivated = true;
        PrefabPositionUpdate();
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
                _prefabForword = _playerTransform.forward;
                _prefabRight = _playerTransform.right;
                _prefabUp = _playerTransform.up;
            }
        }
    }
    private void SetColor(Color color)
    {
        foreach (Material mat in _materials)
        {
            Debug.Log("칼라 세팅");
            mat.SetColor("_Color", color);
        }
    }

    private void ReSetColor()
    {
        for (int i = 0; i < _materials.Count; i++)
        {
            Debug.Log("칼라 되돌리기");
            _materials[i].color = _originColors[i];
        }
    }

    public void OnMoveItem(InputAction.CallbackContext context)
    {
        if (_isPrefabActivated)
            if (context.phase == InputActionPhase.Performed)
            {
                _isItemMoving = true;
                _curMovementInput = context.ReadValue<Vector3>();
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(MoveItemCo());
            }
            else
            {
                _isItemMoving = false;
            }
    }

    IEnumerator MoveItemCo()
    {
        while (_isItemMoving)
        {
            Debug.Log("무브 코루틴");
            Vector3 dir = _prefabForword * _curMovementInput.z + _prefabRight * _curMovementInput.x + _prefabUp * _curMovementInput.y;
            dir *= _moveSpeed;

            _rigidbody.velocity = dir;
            yield return new WaitForFixedUpdate();
        }
    }

    public void PlacePrefab()
    {
        ReSetColor();
        _craftedItemPrefab.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _craftedItemPrefab = null;
        _isPrefabActivated = false;
        _isItemMoving = false;
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void ClearPreview()
    {
        if (_craftedItemPrefab != null)
        {
            ReSetColor();
            Destroy(_craftedItemPrefab);
        }
    }

    //다른 곳에서 할 예정
    //public void SlotClick(int slotNumber)
    //{
    //    _craftingCanvas.SetActive(false);
    //    _craftingCamera.SetActive(true);
    //    _placeItemControl.PreviewItemView(_craftedItems[slotNumber].itemPrefab);
    //    _isPreviewOn = true;

    //}

    //public void OnBuild(InputAction.CallbackContext context)
    //{
    //    if (_isPreviewOn == true)
    //        if (context.phase == InputActionPhase.Started)
    //        {
    //            _placeItemControl.PlacePrefab();
    //            _craftingCanvas.SetActive(true);
    //            _craftingCamera.SetActive(false);
    //            _isPreviewOn = false;
    //        }
    //}
}
