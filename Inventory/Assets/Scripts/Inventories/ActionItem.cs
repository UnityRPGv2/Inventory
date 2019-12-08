using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/RPG.UI.InventorySystem/Action Item"))]
    public class ActionItem : InventoryItem
    {
        [SerializeField] bool _consumable = false;

        public bool isConsumable => _consumable;

        public virtual void Use(GameObject player)
        {

        }
    }
}