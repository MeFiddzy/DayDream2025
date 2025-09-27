using UnityEngine;

public abstract class Item
{
    public abstract string getName();
    public enum UseResult
    {
        SUCCESS,
        FAIL
    }

    public virtual void onStart() {}
    public virtual void onFrame() {}
    public virtual UseResult onUse(Vector2 pos) { return UseResult.SUCCESS; }
    public virtual void onCollect(Vector2 pos) {}
    public virtual void onDrop(Vector2 pos) {}

    public abstract Item getCopy();

    private Sprite m_itemSprite;

    public Sprite getTexture() => m_itemSprite;
}