using System.Collections;
using UnityEngine;
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
    private Material[] _materials;
    private Material[] _originMaterials;

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
        _materials = _meshRenderer.materials;
        _originMaterials = (Material[])_materials.Clone();
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
            mat.SetColor("Color", color);
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
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = _originMaterials[i].color;
        }
        _craftedItemPrefab.GetComponent<Rigidbody>().velocity = Vector3.zero;
        _craftedItemPrefab = null;
        _isPrefabActivated = false;
        _isItemMoving = false;
        StopCoroutine(_coroutine);
    }

    public void ClearPreview()
    {
        if (_craftedItemPrefab != null)
            Destroy(_craftedItemPrefab);
    }

    
}
