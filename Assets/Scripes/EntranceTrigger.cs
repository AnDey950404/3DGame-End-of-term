using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntranceTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject hintUI;          // 拖你的提示文字物件進來（Text/TMP 都可）

    [Header("Load Scene")]
    public string targetSceneName = "Level1";

    private bool playerInside = false;

    void Start()
    {
        if (hintUI != null) hintUI.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;
        if (Keyboard.current == null) return;

        // ✅ 新 Input System：按下空白鍵
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (hintUI != null) hintUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (hintUI != null) hintUI.SetActive(false);
    }
}
