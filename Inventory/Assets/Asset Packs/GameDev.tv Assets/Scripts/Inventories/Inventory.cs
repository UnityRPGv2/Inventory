using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [SerializeField] int inventorySize;

        public InventoryItem[] slots { get; private set; }

        public event Action inventoryUpdated = delegate { };

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        private void Awake() {
            slots = new InventoryItem[inventorySize];
            slots[3] = InventoryItem.GetFromID("2895d758-1928-4352-b531-b3d2cf6456fd");
            slots[7] = InventoryItem.GetFromID("2895d758-1928-4352-b531-b3d2cf6456fd");
            slots[10] = InventoryItem.GetFromID("ffb821ba-a101-4679-9853-d5b577d6696b");
        }

        public bool HasItem(InventoryItem consumeItem)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i], consumeItem))
                {
                    return true;
                }
            }
            return false;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot];
        }

        public void RemoveFromSlot(int slot)
        {
            slots[slot] = null;
            inventoryUpdated();
        }

        public void AddItemToSlot(int slot, InventoryItem item)
        {
            slots[slot] = item;
            inventoryUpdated();
        }

        public object CaptureState()
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i] != null)
                {
                    slotStrings[i] = slots[i].itemID;
                }
            }
            return slotStrings;
        }

        public void RestoreState(object state)
        {
            var slotStrings = (string[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i] = InventoryItem.GetFromID(slotStrings[i]);
            }
            inventoryUpdated();
        }
    }
}