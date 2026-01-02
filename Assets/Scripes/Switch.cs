using UnityEngine;

public class Switch : MonoBehaviour
{
    public Door targetDoor;   // 指向要打開的門
    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;
        if (!other.CompareTag("Player")) return;

        activated = true;
        targetDoor.Open();
    }
}
