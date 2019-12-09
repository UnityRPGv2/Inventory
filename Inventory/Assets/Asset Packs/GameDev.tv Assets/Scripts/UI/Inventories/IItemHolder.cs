using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace GameDevTV.UI.Inventories
{
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}