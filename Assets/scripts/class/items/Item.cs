using System;
using UnityEngine;

public abstract class Item
{
    public abstract string getName();
    public enum UseResult
    {
        SUCCESS,
        FAIL
    }

    public void onStart() {}
    
    public void onFrame() {}
    
    public abstract Item getCopy();

    public Item() { }
    
    private Sprite m_itemSprite;

    public UseResult onUse(Vector2 pos)
    {
        return UseResult.SUCCESS;
    }
    
    public void onCollect(Vector2 pos) {}
    
    public void onDrop(Vector2 pos) {}

    public Sprite getTexture()
    {
        return m_itemSprite;
    }
}