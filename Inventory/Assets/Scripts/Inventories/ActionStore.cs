using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
        private class DockedItemSlot 
        {
            public InventoryItem item;
            public int number;
        }

        public event Action storeUpdated;

        public InventoryItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        public void AddAction(InventoryItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {  
                if (object.ReferenceEquals(item, dockedItems[index].item))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.item = item;
                slot.number = number;
                dockedItems[index] = slot;
            }
            storeUpdated();
        }

        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems.Remove(index);
                }
                storeUpdated();
            }
            
        }

        public bool CanAcceptAction(InventoryItem item, int index)
        {
            return !dockedItems.ContainsKey(index) || object.ReferenceEquals(item, dockedItems[index].item);
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.item.itemID;
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddAction(InventoryItem.GetFromID(pair.Value.itemID), pair.Key, pair.Value.number);
            }
        }
    }
}