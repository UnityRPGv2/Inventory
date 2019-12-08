using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameDevTV.Inventories;
using TMPro;

public class InventoryItemIcon : MonoBehaviour
{
    public void SetItem(InventoryItem item)
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
    }
}
