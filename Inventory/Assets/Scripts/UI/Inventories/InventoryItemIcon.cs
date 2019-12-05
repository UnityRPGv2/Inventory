using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;

public class InventoryItemIcon : MonoBehaviour
{
    Image _iconImage;

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
