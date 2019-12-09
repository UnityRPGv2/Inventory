using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using InventoryExample.Control;

namespace GameDevTV.Inventories
{
    public class Pickup : MonoBehaviour
    {
        InventoryItem _item;
        int _number = 1;

        Inventory _inventory;

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _inventory = player.GetComponent<Inventory>();
        }

        public void PickupItem()
        {
            bool foundSlot = _inventory.AddToFirstEmptySlot(_item, _number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()
        {
            return _inventory.HasSpaceFor(_item);
        }

        public InventoryItem item { get { return _item; } set { _item = value; } }
        public int number { get { return _number; } set { _number = value; } }
    }
}