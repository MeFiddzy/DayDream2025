using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    private Transform m_playerTransform;
    
    private ArmourItem m_equipedArmour;
    
    private List<Item> m_items = new List<Item>();

    public void Awake()
    {
        m_playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    public int inventorySlotsCount = 10;
    public int getItemCount()
    {
        return m_items.Count;
    }
    
    public Item getItem(int index)
    {
        if (index > getItemCount())
            throw new IndexOutOfRangeException();
        
        return m_items[index];
    }

    public void setItem(int index, Item item)
    {
        if (index > getItemCount())
            throw new IndexOutOfRangeException();
        
        m_items[index] = item;
    }

    public void addItem(Item item)
    {
        if (getItemCount() + 1 > inventorySlotsCount)
            throw new IndexOutOfRangeException();
        
        m_items.Add(item);
        item.onCollect(m_playerTransform.position);
    }

    public bool dropItem(int index)
    {
        if (index > getItemCount())
            return false;
        
        m_items.RemoveAt(index);
        
        m_items[index].onDrop(m_playerTransform.position);
        return true;
    }
    
    public Item.UseResult useItem(int index)
    {
        if (index > getItemCount())
            throw new IndexOutOfRangeException();

        if (m_items[index] is ArmourItem armour)
            equipArmour(armour);
        
        return m_items[index].onUse(m_playerTransform.position);
    }

    private void equipArmour(ArmourItem armour)
    {
        m_items.Add(m_equipedArmour);
        
        m_equipedArmour = armour;
    }

    private void Start()
    {
        foreach (Item curItem in m_items)
            curItem.onStart();
    }

    public void Update()
    {
        foreach (Item curItem in m_items)
            curItem.onFrame();
    }
}