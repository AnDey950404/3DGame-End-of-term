using UnityEngine;

public class Door : MonoBehaviour
{
    Collider2D[] cols;
    SpriteRenderer[] renderers;

    void Awake()
    {
        cols = GetComponents<Collider2D>();                 // 門自己身上的全部 Collider
        renderers = GetComponentsInChildren<SpriteRenderer>(); // 門自己/子物件的圖
    }

    public void Open()
    {
        foreach (var c in cols) c.enabled = false;          // ✅ 真正讓它不擋路
        foreach (var r in renderers) r.enabled = false;     // 讓它看不見
    }

    public void Close()
    {
        foreach (var c in cols) c.enabled = true;
        foreach (var r in renderers) r.enabled = true;
    }
}
