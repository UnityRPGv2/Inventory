using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;

namespace RPG.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItem item;

        public Pickup pickup { get => GetComponentInChildren<Pickup>(); }

        public bool isCollected { get => pickup == null; }

        private void Awake() {
            SpawnPickup();
        }

        public object CaptureState()
        {
            return isCollected;
        }

        public void RestoreState(object state)
        {
            bool shouldBeCollected = (bool)state;

            if (shouldBeCollected && !isCollected)
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && isCollected)
            {
                SpawnPickup();
            }
        }

        private void SpawnPickup()
        {   
            var spawnedPickup = item.SpawnPickup(transform.position);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (pickup)
            {
                Destroy(pickup.gameObject);
            }
        }
    }
}