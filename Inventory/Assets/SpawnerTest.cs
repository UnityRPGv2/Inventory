using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.UI.Inventories;
using GameDevTV.Inventories;

public class SpawnerTest : MonoBehaviour
{
    [SerializeField] ItemTooltip tooltipPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        ItemTooltip tooltipInstance = Instantiate(tooltipPrefab, transform);
        tooltipInstance.Setup(InventoryItem.GetFromID("0aa7c8b8-4796-42aa-89d0-9d100ea67d7b"));
    }

}
