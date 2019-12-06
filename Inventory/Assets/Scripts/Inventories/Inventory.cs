using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {

        int coin;
        private List<Pickup> droppedItems = new List<Pickup>();

        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real
        [SerializeField] int inventorySize;

        public InventorySlot[] slots { get; private set; }

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        private void Awake() {
            slots = new InventorySlot[inventorySize];
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public event Action inventoryUpdated = delegate {};

        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);
            print(number);

            if (i < 0)
            {
                return false;
            }

            slots[i].item = item;
            slots[i].number += number;
            inventoryUpdated();
            return true;
        }

        public bool DropItem(InventoryItem item, int number)
        {
            if (item == null) return false;

            var spawnLocation = transform.position;
            SpawnPickup(item, spawnLocation, number);
            inventoryUpdated();

            return true;
        }

        public bool ConsumeItem(InventoryItem consumeItem)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, consumeItem))
                {
                    slots[i].number--;
                    if (slots[i].number <= 0)
                    {
                        slots[i].item = null;
                    }
                    inventoryUpdated();
                    return true;
                }
            }
            return false;
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
            print(i);
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
            if (!item.isStackable)
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

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
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

        private void CaptureInventoryState(IDictionary<string, object> state)
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (slots[i].item != null)
                {
                    slotStrings[i] = slots[i].item.itemID;
                }
            }
            state["inventorySlots"] = slotStrings;
        }

        private void CaptureDropState(IDictionary<string, object> state)
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new Dictionary<string, object>[droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i] = new Dictionary<string, object>();
                droppedItemsList[i]["itemID"] = droppedItems[i].item.itemID;
                droppedItemsList[i]["position"] = new SerializableVector3(droppedItems[i].transform.position);
                droppedItemsList[i]["number"] = droppedItems[i].number;
            }
            state["droppedItems"] = droppedItemsList;
        }

        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }

        private void DeleteAllDrops()
        {
            RemoveDestroyedDrops();
            foreach (var item in droppedItems)
            {
                Destroy(item.gameObject);
            }
        }

        private void RestoreInventory(IReadOnlyDictionary<string, object> state)
        {
            if (!state.ContainsKey("inventorySlots")) return;
            var slotStrings = (string[])state["inventorySlots"];
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i].item = InventoryItem.GetFromID(slotStrings[i]);
            }
        }

        public void AddCoin(int amount)
        {
            coin += amount;
        }

        public int GetCoinAmount()
        {
            return coin;
        }

        public bool IsPlayerCarrying()  // TODO pass paramater
        {
            return hasDeliveryItem;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            CaptureInventoryState(state);
            CaptureDropState(state);
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;
            RestoreInventory(stateDict);
            inventoryUpdated();

            DeleteAllDrops();
            if (stateDict.ContainsKey("droppedItems"))
            {
                var droppedItemsList = (Dictionary<string, object>[])stateDict["droppedItems"];
                foreach (var item in droppedItemsList)
                {
                    var pickupItem = InventoryItem.GetFromID((string)item["itemID"]);
                    Vector3 position = ((SerializableVector3)item["position"]).ToVector();
                    int number = ((int)item["number"]);
                    SpawnPickup(pickupItem, position, number);
                }
            }
        }
    }
}