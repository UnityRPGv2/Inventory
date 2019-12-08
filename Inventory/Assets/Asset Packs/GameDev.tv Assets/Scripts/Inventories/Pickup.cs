using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using InventoryExample.Control;

namespace GameDevTV.Inventories
{
    public class Pickup : MonoBehaviour, IRaycastable
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

        public CursorType GetCursorType()
        {
            if (_inventory.HasSpaceFor(_item))
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.FullPickup;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickupItem();
            }
            return true;
        }

        public InventoryItem item { get { return _item; } set { _item = value; } }
        public int number { get { return _number; } set { _number = value; } }
    }
}