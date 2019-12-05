using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
    public class CoinDisplay : MonoBehaviour
    {

        Inventory playerInventory;
        Text coinText;

        // Use this for initialization
        void Start()
        {
            playerInventory = FindObjectOfType<Inventory>(); // assuming only player
            coinText = GetComponent<Text>();
        }

        // Update is called once per frame
        void Update()
        {
            coinText.text = playerInventory.GetCoinAmount().ToString();
        }
    }
}