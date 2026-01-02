using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // 方式A：按鈕點擊時呼叫
    public void GoToLevel1()
    {
        SceneManager.LoadScene("level 1");
    }

    // 方式B：整個畫面任意點一下就進第一關（可選）
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左鍵/點擊
        {
            SceneManager.LoadScene("level 1");
        }
    }
}