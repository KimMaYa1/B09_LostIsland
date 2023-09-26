using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIManager instance;

    [SerializeField] private GameObject _uiPrefab;
    private GameObject _uiObject;

    private void Awake()
    {
        instance = this;
        _uiObject = Instantiate(_uiPrefab);
        _uiObject.SetActive(false);
    }
}
