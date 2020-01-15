using System.Collections;
using System.Collections.Generic;
using GameDevTV.Core.UI.Dragging;
using GameDevTV.Inventories;
using GameDevTV.UI.Inventories;
using UnityEngine;

public class EquipmentSlotUIChallenge : MonoBehaviour, IDragContainer<InventoryItem>
{
    // CONFIG DATA

    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;

    // STATE
    EquipableItem item = null;

    public void AddItems(InventoryItem item, int number)
    {
        this.item = item as EquipableItem;
        icon.SetItem(item);
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public int GetNumber()
    {
        if (item == null)
        {
            return 0;
        }

        return 1;
    }

    public int MaxAcceptable(InventoryItem item)
    {
        if (!(item is EquipableItem))
        {
            return 0;
        }
        var equipableItem = item as EquipableItem;
        if (equipableItem.GetAllowedEquipLocation() != equipLocation)
        {
            return 0;
        }
        if (this.item != null)
        {
            return 0;
        }
        return 1;
    }

    public void RemoveItems(int number)
    {
        item = null;
        icon.SetItem(null);
    }
}
