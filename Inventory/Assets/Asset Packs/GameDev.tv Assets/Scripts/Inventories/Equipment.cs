using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation, EquipableItem> equippedItems = new Dictionary<EquipLocation, EquipableItem>();

        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipLocation slot)
        {
            if (!equippedItems.ContainsKey(slot))
            {
                return null;
            }

            return equippedItems[slot];
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            equippedItems[slot] = item;

            equipmentUpdated();
        }

        public void RemoveItem(EquipLocation slot)
        {
            equippedItems.Remove(slot);
            equipmentUpdated();
        }

        public object CaptureState()
        {
            var equippedItemsForSerialization = new Dictionary<EquipLocation, string>();
            foreach (var pair in equippedItems)
            {
                equippedItemsForSerialization[pair.Key] = pair.Value.GetItemID();
            }
            return equippedItemsForSerialization;
        }

        public void RestoreState(object state)
        {
            equippedItems = new Dictionary<EquipLocation, EquipableItem>();

            var equippedItemsForSerialization = (Dictionary<EquipLocation, string>)state;

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