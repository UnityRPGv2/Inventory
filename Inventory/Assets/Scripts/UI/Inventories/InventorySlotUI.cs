using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Inventories;
using GameDevTV.Core.UI.Dragging;

namespace InventoryExample.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon;

        public int index { get; set; }

        Inventory _inventory;
        InventoryItem _item;

        public Inventory inventory { set { _inventory = value; } }

        public void SetItem(InventoryItem item)
        {
            _icon.SetItem(item);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (_inventory.GetItemInSlot(index) == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _inventory.AddItemToSlot(index, item);
        }

        public InventoryItem GetItem()
        {
            return _inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return 1;
        }

        public void RemoveItems(int number)
        {
            _inventory.RemoveFromSlot(index);
        }
    }
}