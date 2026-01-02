using UnityEngine;

public class Apple : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 只接受玩家（避免牆、身體、其他 trigger 亂吃）
        if (!other.CompareTag("Player")) return;

        // 有時候碰到的是玩家的子物件 Collider，所以用 InParent 最穩
        PlayerMove player = other.GetComponentInParent<PlayerMove>();

        if (player == null)
        {
            Debug.LogWarning("Apple touched something tagged Player but no PlayerMove found in parent.");
            return;
        }

        Debug.Log("Apple eaten -> AddBody()");
        player.AddBody();

        Destroy(gameObject);
    }
}
