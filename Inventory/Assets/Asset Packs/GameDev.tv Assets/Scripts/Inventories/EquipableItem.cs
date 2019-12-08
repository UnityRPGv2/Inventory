using UnityEngine;

namespace GameDevTV.Inventories
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        public enum EquipLocation
        {
            Helmet,
            Necklace,
            Body,
            Trousers,
            Boots,
            Weapon,
            Shield,
            Gloves,
        }
        [SerializeField] EquipLocation _allowedEquipLocation;

        public EquipLocation allowedEquipLocation
        {
            get
            {
                return _allowedEquipLocation;
            }
        }

        virtual public void Equip(EquipLocation location, Equipment equipement)
        {

        }

    }
}