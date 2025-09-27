using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public int itemID;

    private InventoryManager invManager;

    private void Awake()
    {
        invManager = GameObject.Find("Player").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collision2D other)
    {
        invManager.addItem(UItems.getItemById(itemID));
    }
}
