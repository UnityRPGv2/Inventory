using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}