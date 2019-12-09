using System;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [SerializeField] int inventorySize;

        InventorySlot[] slots;

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        public event Action inventoryUpdated = delegate { };

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        private void Awake() {
            slots = new InventorySlot[inventorySize];
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public int GetSize()
        {
            return slots.Length;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            inventoryUpdated();
            return true;
        }

        public bool HasItem(InventoryItem consumeItem)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, consumeItem))
                {
                    return true;
                }
            }
            return false;
        }

        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable())
            {
                return -1;
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    return i;
                }
            }
            return -1;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
        }

        public int GetNumberInSlot(int slot)
        {
            return slots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0) 
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            inventoryUpdated();
        }

        public void AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null)
            {
                AddToFirstEmptySlot(item, number);
                return;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            inventoryUpdated();
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }
    
        public object CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i].itemID = slots[i].item.GetItemID();
                    slotStrings[i].number = slots[i].number;
                }
            }
            return slotStrings;
        }

        public void RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotStrings[i].itemID);
                slots[i].number = slotStrings[i].number;
            }
            inventoryUpdated();
        }
    }
}