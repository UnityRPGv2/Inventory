using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Core.UI.Tooltips;

namespace GameDevTV.UI.Inventories
{
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip) return;

            var item = GetComponent<IItemHolder>().GetItem();
            if (!item) return;

            itemTooltip.title = item.GetDisplayName();
            itemTooltip.body = item.GetDescription();
        }
    }
}