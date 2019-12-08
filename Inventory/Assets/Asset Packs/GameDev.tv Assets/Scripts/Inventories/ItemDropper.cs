using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {

        private List<Pickup> droppedItems = new List<Pickup>();

        public bool DropItem(InventoryItem item, int number)
        {
            if (item == null) return false;

            var spawnLocation = transform.position;
            SpawnPickup(item, spawnLocation, number);

            return true;
        }

        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
        }


        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
        }
        public object CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new DropRecord[droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = droppedItems[i].item.itemID;
                droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
                droppedItemsList[i].number = droppedItems[i].number;
            }
            return droppedItemsList;
        }

        public void RestoreState(object state)
        {
            var droppedItemsList = (DropRecord[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
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
    }
}