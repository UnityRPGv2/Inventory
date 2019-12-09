using UnityEngine;

namespace GameDevTV.Inventories
{
    public class Pickup : MonoBehaviour
    {
        InventoryItem item;
        int number = 1;

        Inventory inventory;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            this.number = number;
        }

        public InventoryItem GetItem()
        {
            return item;
        }

        public int GetNumber()
        {
            return number;
        }

        public void PickupItem()
        {
            bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return inventory.HasSpaceFor(item);
        }
    }
}