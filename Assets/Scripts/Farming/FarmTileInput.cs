using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTileInput : MonoBehaviour
{
    [Header("References")]
    public FarmTileManager farmTileManager;
    public Transform player;
    public float interactionRadius = 1.5f;   // Max distance player can reach

    private Camera mainCamera;

    private void Start()
    {
        if (farmTileManager == null)
        {
            farmTileManager = FindObjectOfType<FarmTileManager>();
            if (farmTileManager == null)
                Debug.LogError("FarmTileManager not found in scene!");
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("No Main Camera found in scene!");

        // Wait for player to spawn dynamically
        StartCoroutine(FindPlayerRoutine());
    }

    private IEnumerator FindPlayerRoutine()
    {
        while (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log($"✅ Found player at runtime: {player.name}");
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        // Editor/Mouse
        if (Input.GetMouseButtonDown(0))
            HandleInput(Input.mousePosition);

        // Mobile/Touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            HandleInput(Input.GetTouch(0).position);
    }

    private void HandleInput(Vector2 screenPos)
{
    if (mainCamera == null || farmTileManager == null)
        return;

    // ✅ Check if the tilemap still exists
    if (farmTileManager.farmTilemap == null)
    {
        Debug.LogWarning("⚠️ No active tilemap to interact with (probably left chunk).");
        return;
    }

    // ✅ Prevent using destroyed tilemaps
    if (farmTileManager.farmTilemap.Equals(null))
    {
        Debug.LogWarning("⚠️ Tilemap reference destroyed — waiting for new chunk.");
        return;
    }

    Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
    worldPos.z = 0f;

    Vector3Int cellPos = farmTileManager.farmTilemap.WorldToCell(worldPos);
    Vector3 cellCenter = farmTileManager.farmTilemap.GetCellCenterWorld(cellPos);

    float distance = Vector3.Distance(player.position, cellCenter);
    Debug.Log($"🧭 Player at {player.position} | Tile center {cellCenter} | Distance {distance:F2}");

    if (distance > interactionRadius)
    {
        Debug.Log("Too far from tile!");
        return;
    }

    if (ToolSelectionManager.Instance == null || ToolSelectionManager.Instance.selectedTool == null)
    {
        Debug.LogWarning("No tool selected!");
        return;
    }

    if (TileInteractionManager.Instance == null)
    {
        Debug.LogError("TileInteractionManager not found!");
        return;
    }

    TileInteractionManager.Instance.InteractWithTile(cellCenter);
    Debug.Log($"✅ Interacted with tile at {cellPos} using tool {ToolSelectionManager.Instance.selectedTool.toolType}");
}

}
