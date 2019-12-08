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
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindEmptySlot() >= 0;
        }

        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            int i = FindEmptySlot();

            if (i < 0)
            {
                return false;
            }

            slots[i] = item;
            inventoryUpdated();
            return true;
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
            if (slots[slot] != null)
            {
                AddToFirstEmptySlot(item);
                return;
            }

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