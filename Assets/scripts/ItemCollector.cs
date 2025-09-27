using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public int itemID;

    public void OnTriggerEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<InventoryManager>().addItem(UItems.getItemById(itemID));
        print($"Picked up {UItems.getItemById(itemID).getName()}");
    }
}
