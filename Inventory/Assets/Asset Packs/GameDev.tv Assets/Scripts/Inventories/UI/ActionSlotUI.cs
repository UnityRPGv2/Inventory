using System.Collections;
using System.Collections.Generic;
using RPG.Core.UI.Dragging;
using RPG.Inventories;
using RPG.SpecialActions;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ActionSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon _icon;
        [SerializeField] int index = 0;

        SpecialAbilities _abilitiesStore;

        public InventoryItem item { get => _abilitiesStore.GetAbility(index); }

        private void Awake() {
            _abilitiesStore = GameObject.FindGameObjectWithTag("Player").GetComponent<SpecialAbilities>();
            _abilitiesStore.OnAbilitiesUpdated += UpdateIcon;
        }

        bool IDragContainer<InventoryItem>.CanAcceptItem(InventoryItem item)
        {
            return item is ActionConfig;
        }

        InventoryItem IDragContainer<InventoryItem>.ReplaceItem(InventoryItem item)
        {
            _abilitiesStore.SetAbility(item as ActionConfig, index);

            // So that it isn't removed from previous place.
            return item;
        }

        void UpdateIcon()
        {
            _icon.SetItem(item);
        }
    }
}
