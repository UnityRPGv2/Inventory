using UnityEngine;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Action Item"))]
    public class ActionItem : InventoryItem
    {
        [SerializeField] bool _consumable = false;

        public bool isConsumable => _consumable;

        public virtual void Use(GameObject player)
        {
            Debug.Log("Using action: " + this);
        }
    }
}