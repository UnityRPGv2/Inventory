using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace InventoryExample.UI.Inventories
{
    public interface IItemHolder
    {
        InventoryItem item { get; }
    }
}