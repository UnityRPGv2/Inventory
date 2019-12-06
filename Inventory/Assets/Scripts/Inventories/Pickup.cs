using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using InventoryExample.Control;

namespace RPG.Inventories
{
    public class Pickup : MonoBehaviour, IRaycastable
    {
        InventoryItem _item;
        int _number = 1;
        
        public void PickupItem()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var inventory = player.GetComponent<Inventory>();
            bool foundSlot = inventory.AddToFirstEmptySlot(_item, _number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
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