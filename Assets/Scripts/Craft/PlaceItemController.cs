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
    private Vector3 _prefabForword;
    private Vector3 _prefabRight;
    private bool _isPrefabActivated = false;
    private bool _isItemMoving = false;
    private float _itemMoveSpeed = 0.1f;
    private Coroutine _coroutine;
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

        _prefabForword = _camera.transform.forward;
        _prefabForword.y = 0f;
        _prefabForword.Normalize();
        _prefabRight = _camera.transform.right;
        _prefabRight.y = 0f;
        _prefabRight.Normalize();

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
                Debug.Log("칼라 세팅");
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
                Debug.Log("칼라 되돌리기");
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
            Vector3 dir = _prefabForword * _curMovementInput.z + _prefabRight * _curMovementInput.x + new Vector3(0, 1, 0) * _curMovementInput.y;
            dir *= _itemMoveSpeed;
            _craftedItemPrefab.transform.position += dir;

            yield return new WaitForFixedUpdate();
        }
    }

    public void PlacePrefab()
    {
        ReSetColor();
        _craftedItemPrefab = null;
        _isPrefabActivated = false;
        _isItemMoving = false;
        _craftedItemPrefab.GetComponent<NavMeshSurface>().BuildNavMesh();
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

}
