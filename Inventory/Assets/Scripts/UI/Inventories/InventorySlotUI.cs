﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Inventories;
using GameDevTV.Core.UI.Dragging;

namespace GameDevTV.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon;

        public int index { get; set; }

        Inventory _inventory;
        InventoryItem _item;

        public Inventory inventory { set { _inventory = value; } }

        public InventoryItem item { 
            get => _inventory.GetItemInSlot(index);
        }

        public void SetItem(InventoryItem item, int number)
        {
            _icon.SetItem(item, number);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (_inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _inventory.AddItemToSlot(index, item, number);
        }

        public InventoryItem GetItem()
        {
            return _inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return _inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(index, number);
        }
    }
}