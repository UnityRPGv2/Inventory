using UnityEngine;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        [SerializeField] EquipLocation allowedEquipLocation;

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
    }
}