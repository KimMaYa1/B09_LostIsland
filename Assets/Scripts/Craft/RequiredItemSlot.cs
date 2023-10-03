using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequiredItemSlot : MonoBehaviour
{
    [SerializeField] Image _requiredItemImage;
    [SerializeField] private TMP_Text _requiredItemName;
    [SerializeField] private TMP_Text _curCount;
    [SerializeField] private TMP_Text _requiredCount;
    private int _curAmount = 0;
    private int _reqAmount = 1;

    public void SetRequiredItemSlot(Sprite sprite, string name, string curCount, string requiredCount)
    {
        _requiredItemImage.sprite = sprite;
        _requiredItemName.text = name;
        _curCount.text = curCount;
        _requiredCount.text = requiredCount;
        SetCountColor(curCount, requiredCount);
    }

    private void SetCountColor(string curCount, string requiredCount)
    {
        _curAmount = int.Parse(curCount);
        _reqAmount = int.Parse(requiredCount);

        if (_curAmount >= _reqAmount)
        {
            _curCount.color = Color.green;
            _requiredCount.color = Color.green;
        }
        else
        {
            _curCount.color = Color.red;
            _requiredCount.color = Color.red;
        }
    }

    public void ResetRequiredItemSlot()
    {
        _requiredItemImage.sprite = null;
        _requiredItemName.text = "";
        _curCount.text = "";
        _requiredCount.text = "";
        _curAmount = 0;
        _reqAmount = 1;
    }

    public int AvaiableAmount()
    {
        return _curAmount / _reqAmount;
    }
}
