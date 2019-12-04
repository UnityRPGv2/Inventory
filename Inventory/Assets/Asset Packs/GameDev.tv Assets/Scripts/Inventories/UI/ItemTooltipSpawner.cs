using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core.UI.Tooltips;

namespace RPG.UI.Inventories
{
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip) return;

            var item = GetComponent<IItemHolder>().item;
            if (!item) return;

            itemTooltip.title = item.displayName;
            itemTooltip.body = item.description;
        }
    }
}