using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameDevTV.Core.UI.Dragging;

namespace InventoryExample.UI.Inventories
{
    public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
    {
        [SerializeField] InventoryItemIcon _icon;

        public int index { get; set; }

        public int MaxAcceptable(Sprite item)
        {
            if (_icon.GetItem() == null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(Sprite item, int number)
        {
            _icon.SetItem(item);
        }

        public Sprite GetItem()
        {
            return _icon.GetItem();
        }

        public int GetNumber()
        {
            if (_icon.GetItem() == null)
            {
                return 0;
            }
            return 1;
        }

        public void RemoveItems(int number)
        {
            _icon.SetItem(null);
        }
    }
}