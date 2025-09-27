using UnityEngine;

public abstract class Item
{
    public enum UseResult
    {
        SUCCESS,
        FAIL
    }
    
    private Texture2D m_itemTexture;

    public abstract UseResult onUse(Vector2 pos);
    
    public abstract void onCollect(Vector2 pos);
    
    public abstract void onDrop(Vector2 pos);

    public Texture2D getTexture()
    {
        return m_itemTexture;
    }
}