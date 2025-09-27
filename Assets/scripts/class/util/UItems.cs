using System.Collections.Generic;
using UnityEngine;

public static class UItems
{
    public static Item getItemById(int id)
    {
        return new List<Item> {
            new ShieldItem()
        } [id].getCopy();
    }
}