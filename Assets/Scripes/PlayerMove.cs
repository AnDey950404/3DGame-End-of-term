using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Grid Move")]
    public float gridSize = 1f;
    public float moveSpeed = 8f;
    public Vector3 startPosition = new Vector3(-1.5f, -0.5f, 0f);

    [Header("Wall Blocking")]
    public LayerMask wallLayer;        // Inspector 勾 Wall
    public float checkRadius = 0.35f;  // 0.30 ~ 0.45 之間調
    public bool bumpOnBlocked = true;  // 撞牆是否做頂一下回饋
    public float bumpDistance = 0.12f;
    public float bumpSpeed = 18f;

    [Header("Snake Body")]
    public GameObject bodyPrefab;      // 拖 BodySegment prefab
    public int startBodyCount = 0;     // 初始幾節

    private bool isMoving = false;

    // 記錄走過的格子位置（0 是頭）
    private readonly List<Vector3> positionHistory = new List<Vector3>();
    private readonly List<Transform> bodyList = new List<Transform>();

    void Start()
    {
        transform.position = startPosition;

        positionHistory.Clear();
        positionHistory.Add(transform.position);

        for (int i = 0; i < startBodyCount; i++)
            AddBody();
    }

    void Update()
    {
        if (isMoving) return;
        if (Keyboard.current == null) return;

        Vector2 dir = Vector2.zero;
        if (Keyboard.current.wKey.wasPressedThisFrame) dir = Vector2.up;
        else if (Keyboard.current.sKey.wasPressedThisFrame) dir = Vector2.down;
        else if (Keyboard.current.aKey.wasPressedThisFrame) dir = Vector2.left;
        else if (Keyboard.current.dKey.wasPressedThisFrame) dir = Vector2.right;

        if (dir == Vector2.zero) return;

        Vector3 targetPos = transform.position + (Vector3)(dir * gridSize);

        // ✅ 牆壁檢查：目標格是否有牆
        bool blocked = Physics2D.OverlapCircle(targetPos, checkRadius, wallLayer) != null;
        if (blocked)
        {
            if (bumpOnBlocked) StartCoroutine(BumpBlocked(dir));
            return;
        }

        StartCoroutine(MoveTo(targetPos));
    }

    System.Collections.IEnumerator MoveTo(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;

        // 🐍 記錄新位置並更新身體
        positionHistory.Insert(0, transform.position);

        for (int i = 0; i < bodyList.Count; i++)
        {
            // i+1：第1節身體跟著頭的上一個位置
            if (i + 1 < positionHistory.Count)
                bodyList[i].position = positionHistory[i + 1];
        }

        // 只保留需要的歷史長度（頭 + 身體數）
        int keep = bodyList.Count + 1;
        if (positionHistory.Count > keep)
            positionHistory.RemoveRange(keep, positionHistory.Count - keep);

        isMoving = false;
    }

    System.Collections.IEnumerator BumpBlocked(Vector2 dir)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 bumpPos = startPos + (Vector3)(dir.normalized * bumpDistance);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * bumpSpeed;
            transform.position = Vector3.Lerp(startPos, bumpPos, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * bumpSpeed;
            transform.position = Vector3.Lerp(bumpPos, startPos, t);
            yield return null;
        }

        transform.position = startPos;
        isMoving = false;
    }

    // 🍎 被蘋果呼叫：加一節身體
    public void AddBody()
    {
        if (bodyPrefab == null)
        {
            Debug.LogError("Body Prefab is NULL! Please assign BodySegment prefab in PlayerMove inspector.");
            return;
        }

        GameObject body = Instantiate(bodyPrefab);
        body.name = $"Body_{bodyList.Count + 1}";

        // 生成在目前尾巴位置（沒有尾巴就生成在頭的位置）
        Vector3 tailPos = positionHistory[positionHistory.Count - 1];
        body.transform.position = tailPos;

        bodyList.Add(body.transform);

        // 增長後補一格歷史，避免下一次更新時跳動
        positionHistory.Add(tailPos);

        Debug.Log($"Body added. Total segments: {bodyList.Count}");
    }
}
