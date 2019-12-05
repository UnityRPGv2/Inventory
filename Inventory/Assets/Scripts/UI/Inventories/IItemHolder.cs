using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public interface IItemHolder
    {
        InventoryItem item { get; }
    }
}