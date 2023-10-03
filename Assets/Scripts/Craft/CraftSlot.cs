using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public void SetSlot(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    public void ClearSlot()
    {
        _icon.sprite = null;
    }
}
