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

        public void CaptureState(IDictionary<string, object> state)
        {
            state["wasCollected"] = isCollected;
        }

        public void RestoreState(IReadOnlyDictionary<string, object> state)
        {
            bool shouldBeCollected = false;
            if (state.ContainsKey("wasCollected"))
            {
                shouldBeCollected = (bool)state["wasCollected"];
            }

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