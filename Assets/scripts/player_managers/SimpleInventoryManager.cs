using UnityEngine;

public class SimpleInventoryManager : MonoBehaviour
{
    public enum SimpleItem
    {
        SWORD,
        SHIELD,
        NONE = 0
    }
    
    private SimpleItem m_equipedItem;
    private Transform m_playerTransform;
    [SerializeField] private GameObject m_shieldPrefab;
    [SerializeField] private GameObject m_swordPrefab;
    
    private GameObject m_eqItem;
    private Transform m_eqItemTransform;
    
    private void Awake()
    {
        m_playerTransform = GameObject.Find("Player").transform;
        m_equipedItem = SimpleItem.NONE;
    }

    public void setItem(int itemID)
    {
        if (m_eqItem != null)
        {
            Destroy(m_eqItem);
            print("not equipped");
        }
        else
        {
            m_equipedItem = (itemID == 1 ? SimpleItem.SWORD : SimpleItem.SHIELD);
            print("equipped");
        }

        
        GameObject prefab = new GameObject();
        if (itemID == 1)
        {
            prefab = m_swordPrefab;
        }
        else if (itemID == 2)
        {
            prefab = m_shieldPrefab;
        }

        if (itemID != 0)
        {
            m_eqItem = Instantiate(prefab, m_playerTransform);


            m_eqItem.GetComponent<SpriteRenderer>().sortingOrder = 10;

            m_eqItem.transform.localPosition = Vector3.zero;
            m_eqItemTransform = m_eqItem.transform;

            m_eqItemTransform.rotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        
    }
}