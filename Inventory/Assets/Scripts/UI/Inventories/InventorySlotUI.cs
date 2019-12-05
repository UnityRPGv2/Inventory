using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.Inventories;
using RPG.Core.UI.Dragging;

namespace RPG.UI.Inventories
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

        public void SetItem(InventoryItem item)
        {
            _icon.SetItem(item);
        }

        public InventoryItem ReplaceItem(InventoryItem item)
        {
            return _inventory.ReplaceItemInSlot(item, index);
        }

        public bool CanAcceptItem(InventoryItem item)
        {
            return true;
        }
    }
}