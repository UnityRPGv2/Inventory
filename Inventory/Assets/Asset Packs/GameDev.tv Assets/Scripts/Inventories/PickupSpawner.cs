using UnityEngine;
using GameDevTV.Saving;

namespace GameDevTV.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItem item;
        [SerializeField] int number = 1;

        public Pickup GetPickup() 
        { 
            return GetComponentInChildren<Pickup>();
        }

        public bool isCollected() 
        { 
            return GetPickup() == null;
        }

        private void Awake() 
        {
            // Spawn in Awake so can be destroyed by save system after.
            SpawnPickup();
        }

        public object CaptureState()
        {
            return isCollected();
        }

        public void RestoreState(object state)
        {
            bool shouldBeCollected = (bool)state;

            if (shouldBeCollected && !isCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && isCollected())
            {
                SpawnPickup();
            }
        }

        private void SpawnPickup()
        {   
            var spawnedPickup = item.SpawnPickup(transform.position, number);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }
    }
}