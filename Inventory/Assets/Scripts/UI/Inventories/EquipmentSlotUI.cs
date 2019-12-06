using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IDragDestination<InventoryItem>, IDragSource<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon;
        [SerializeField] EquipableItem.EquipLocation equipLocation;

        public int index { get; set; }

        EquipableItem _item;

        Equipment _playerEquipment;

        public EquipableItem item
        {
            get => _playerEquipment.GetItemInSlot(equipLocation);
        }

        private void Start() {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerEquipment = player.GetComponent<Equipment>();
            _playerEquipment.equipmentUpdated += RedrawUI;
            RedrawUI();
        }

        public void RedrawUI()
        {
            _icon.SetItem(_playerEquipment.GetItemInSlot(equipLocation));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (GetItem() != null) return 0;
            if (equipableItem.allowedEquipLocation != equipLocation) return 0;

            return 1;
        }

        public void AddItems(InventoryItem item, int number)
        {
            _playerEquipment.AddItem(equipLocation, (EquipableItem) item);
        }

        public InventoryItem GetItem()
        {
            return _playerEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void RemoveItems(int number)
        {
            _playerEquipment.RemoveItem(equipLocation);
        }
    }
}