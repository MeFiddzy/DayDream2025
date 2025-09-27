using System;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public int itemID;
    private static float s_timeFormStart = 0.0f;

    private void Update()
    {
        s_timeFormStart += Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var mang = GameObject.Find("Player").GetComponent<SimpleInventoryManager>();
        
        if (mang != null&&s_timeFormStart>0.5f)
        {
            mang.setItem(itemID);
        }
    }
}
