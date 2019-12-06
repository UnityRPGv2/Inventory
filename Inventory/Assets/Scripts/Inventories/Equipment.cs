using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipableItem.EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipableItem.EquipLocation, EquipableItem>();

        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipableItem.EquipLocation slot)
        {
            if (!equippedItems.ContainsKey(slot))
            {
                return null;
            }

            return equippedItems[slot];
        }

        public void AddItem(EquipableItem.EquipLocation slot, EquipableItem item)
        {
            equippedItems[slot] = item;

            if (item)
            {
                item.Equip(slot, this);
            }
            equipmentUpdated();
        }

        public void RemoveItem(EquipableItem.EquipLocation slot)
        {
            equippedItems[slot] = null;
            equipmentUpdated();
        }

        public object CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipableItem.EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.itemID;
            }
            return equippedItemsForSerialization;
        }

        public void RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipableItem.EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipableItem.EquipLocation, string>)state;

            foreach (var pair in equippedItemsForSerialization)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(pair.Value);
                if (item != null)
                {
                    equippedItems[pair.Key] = item;
                }
            }
        }
    }
}