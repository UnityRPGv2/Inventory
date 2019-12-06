using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core.UI.Dragging;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragContainer<InventoryItem>
    {
        public InventoryItem ReplaceItem(InventoryItem item)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Inventory>().DropItem(item);
            return null;
        }

        public bool CanAcceptItem(InventoryItem item)
        {
            return true;
        }
    }
}