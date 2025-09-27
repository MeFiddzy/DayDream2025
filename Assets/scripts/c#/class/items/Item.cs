using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum UseResult
    {
        SUCCESS,
        FAIL
    }
    
    private Texture2D m_itemTexture;

    public UseResult onUse(Vector2 pos, GameObject player)
    {
        return UseResult.SUCCESS;
    }
    
    public void onCollect(Vector2 pos, GameObject player) {}
    
    public void onDrop(Vector2 pos, GameObject player) {}

    public Texture2D getTexture()
    {
        return m_itemTexture;
    }
}