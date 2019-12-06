using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using TMPro;

public class InventoryItemIcon : MonoBehaviour
{
    [SerializeField] GameObject _textContainer;
    [SerializeField] TextMeshProUGUI _itemNumber;

    public void SetItem(InventoryItem item)
    {
        SetItem(item, 0);
    }

    public void SetItem(InventoryItem item, int number)
    {
        var iconImage = GetComponent<Image>();
        if (item == null)
        {
            iconImage.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item.icon;
        }

        if (_itemNumber)
        {
            if (number <= 1)
            {
                _textContainer.SetActive(false);
            }
            else
            {
                _textContainer.SetActive(true);
                _itemNumber.text = number.ToString();
            }
        }
    }
}
