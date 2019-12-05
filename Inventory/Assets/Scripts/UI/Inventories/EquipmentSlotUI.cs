using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IDragContainer<InventoryItem>
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

        public InventoryItem ReplaceItem(InventoryItem item)
        {
            return _playerEquipment.ReplaceItemInSlot(equipLocation, (EquipableItem)item);
        }

        public bool CanAcceptItem(InventoryItem item)
        {
            if (!(item is EquipableItem))
            {
                return false;
            }
            var equipableItem = (EquipableItem)item;
            return equipableItem.allowedEquipLocation == equipLocation;
        }
    }
}