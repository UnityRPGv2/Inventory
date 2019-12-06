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

        public event Action inventoryUpdated = delegate {};

        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i].item, item))
                {
                    slots[i].number++;
                    inventoryUpdated();
                    return true;
                }
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    slots[i].item = item;
                    slots[i].number = 1;
                    inventoryUpdated();
                    return true;
                }               
            }
            return false;
        }

        public bool DropItem(InventoryItem item)
        {
            if (item == null) return false;

            var spawnLocation = transform.position;
            SpawnPickup(item, spawnLocation);

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

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation)
        {
            var pickup = item.SpawnPickup(spawnLocation);
            droppedItems.Add(pickup);
        }

        public InventoryItem ReplaceItemInSlot(InventoryItem item, int slot)
        {
            var oldItem = slots[slot].item;
            slots[slot].item = item;
            inventoryUpdated();
            return oldItem;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot].item;
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
                    SpawnPickup(pickupItem, position);
                }
            }
        }
    }
}