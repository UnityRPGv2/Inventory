using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemIcon : MonoBehaviour
{
    public void SetItem(Sprite item)
    {
        var iconImage = GetComponent<Image>();
        if (item == null)
        {
            iconImage.enabled = false;
            iconImage.sprite = null;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item;
        }
    }

    public Sprite GetItem()
    {
        var iconImage = GetComponent<Image>();
        return iconImage.sprite;
    }
}
