using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GameDevTV.Inventories
{
    public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        public enum Rarity
        {
            Household,
            Common,
            Uncommon,
            Rare,
            Legendary,
            Mythical
        }

        [SerializeField] string _itemID;
        [SerializeField] float _baseCost;
        [SerializeField] Rarity _rarity;
        [SerializeField] int _level;
        [SerializeField] string _displayName;
        [TextArea]
        [SerializeField] string _description;
        [SerializeField] Sprite _icon;
        [SerializeField] GameObject _pickup;
        [SerializeField] bool _stackable = false;

        static Dictionary<string, InventoryItem> itemLookupCache;

        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError(string.Format("Looks like there's a duplicate GameDevTV.UI.InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        public string itemID => _itemID;
        public Sprite icon => _icon;
        public string displayName => _displayName;
        public string description => _description;
        public bool isStackable => _stackable;

        public Pickup SpawnPickup(Vector3 position, int number)
        {
            var pickupGameObject = Instantiate(_pickup);
            var pickup = pickupGameObject.AddComponent<Pickup>();
            pickup.transform.position = position;
            pickup.item = this;
            pickup.number = number;
            return pickup;
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(_itemID))
            {
                _itemID = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
