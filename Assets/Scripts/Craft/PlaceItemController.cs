using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceItemController : MonoBehaviour
{

    [SerializeField] private Camera _camera;

    [SerializeField] private Transform _placeTransform;
    [SerializeField] private Transform _playerTransform;

    private GameObject _craftedItemPrefab;

    [SerializeField] private LayerMask layerMask;
    private RaycastHit hit;
    private float range = 10f;

    private Vector3 _curMovementInput;
    private Vector2 _curRotateInput;
    private Vector3 _prefabForword;
    private Vector3 _prefabRight;
    private bool _isPrefabActivated = false;
    private bool _isItemMoving = false;
    private bool _isItemRotating = false;
    private float _itemMoveSpeed = 0.1f;
    private float _itemRotateSpeed = 2f;
    private float _itemPrefabYRot = 0;
    private Coroutine _coroutineMove;
    private Coroutine _coroutineRotate;
    private MeshRenderer[] _meshRenderers;
    private List<Material> _materials = new List<Material>();
    private List<Color> _originColors = new List<Color>();

    private void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        _prefabForword = _camera.transform.forward;
        _prefabForword.y = 0f;
        _prefabForword.Normalize();
        _prefabRight = _camera.transform.right;
        _prefabRight.y = 0f;
        _prefabRight.Normalize();
    }

    public void PreviewItemView(GameObject itemPrefab)
    {
        if (_craftedItemPrefab == null)
            _craftedItemPrefab = Instantiate(itemPrefab, _playerTransform.position + _playerTransform.forward, Quaternion.identity);

        _meshRenderers = _craftedItemPrefab.GetComponentsInChildren<MeshRenderer>();
        _materials.Clear();
        _originColors.Clear();
        for (int j = 0; j < _meshRenderers.Length; j++)
        {
            _materials = _meshRenderers[j].materials.ToList();
            for (int i = 0; i < _materials.Count; i++)
            {
                _originColors.Add(_materials[i].color);
            }
        }

        SetColor(Color.green);

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
            }
        }
    }

    private void SetColor(Color color)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            _materials = meshRenderer.materials.ToList();
            foreach (Material mat in _materials)
            {
                mat.SetColor("_Color", color);
            }
        }
    }

    private void ReSetColor()
    {
        for (int j = 0; j < _meshRenderers.Length; j++)
        {
            _materials = _meshRenderers[j].materials.ToList();
            for (int i = 0; i < _materials.Count; i++)
            {
                _materials[i].color = _originColors[(1 + i) * j];
            }
        }
    }

    public void OnMoveItem(InputAction.CallbackContext context)
    {
        if (_isPrefabActivated)
            if (context.phase == InputActionPhase.Performed)
            {
                _isItemMoving = true;
                _curMovementInput = context.ReadValue<Vector3>();
                if (_coroutineMove != null)
                    StopCoroutine(_coroutineMove);
                _coroutineMove = StartCoroutine(MoveItemCo());
            }
            else
            {
                if (_coroutineMove != null)
                {
                    StopCoroutine(_coroutineMove);
                    _coroutineMove = null;
                }
                _isItemMoving = false;
            }
    }

    IEnumerator MoveItemCo()
    {
        while (_isItemMoving)
        {
            Vector3 dir = _prefabForword * _curMovementInput.z + _prefabRight * _curMovementInput.x + new Vector3(0, 1, 0) * _curMovementInput.y;
            dir *= _itemMoveSpeed;
            _craftedItemPrefab.transform.position += dir;

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnRotateItem(InputAction.CallbackContext context)
    {
        if (_isPrefabActivated)
            if (context.phase == InputActionPhase.Performed)
            {
                _isItemRotating = true;
                _curRotateInput = context.ReadValue<Vector2>();
                if (_coroutineRotate != null)
                    StopCoroutine(_coroutineRotate);
                _coroutineRotate = StartCoroutine(RotateItemCo());
            }
            else
            {
                _isItemRotating = false;
            }
    }

    IEnumerator RotateItemCo()
    {
        while (_isItemRotating)
        {
            _itemPrefabYRot += _curRotateInput.x * _itemRotateSpeed;
            _craftedItemPrefab.transform.localEulerAngles = new Vector3(0, _itemPrefabYRot, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    public void PlacePrefab()
    {
        ReSetColor();
        _craftedItemPrefab = null;
        _isPrefabActivated = false;
        _isItemMoving = false;
        if (_coroutineMove != null)
            StopCoroutine(_coroutineMove);
        if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);
    }

    public void ClearPreview()
    {
        if (_coroutineMove != null)
            StopCoroutine(_coroutineMove);
        if (_coroutineRotate != null)
            StopCoroutine(_coroutineRotate);

        if (_craftedItemPrefab != null)
        {
            ReSetColor();
            Destroy(_craftedItemPrefab);
        }
    }
}
