using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FarmTileManager : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap farmTilemap;

    [Header("Soil Tiles")]
    public TileBase defaultTile;
    public TileBase plowedTile;
    public TileBase seededTile;
    public TileBase wateredSeedTile;
    public Tilemap farmTile;

    [Header("Prefabs")]
    public GameObject cropPrefab;

    [Header("Player")]
    public Animator playerAnimator; 

    private Dictionary<Vector3Int, FarmTileState> tileStates = new();
    private Dictionary<Vector3Int, CropsBehaviour> plantedCrops = new();

    public InventoryUI inventoryUI;

    public static FarmTileManager Instance;

private void Awake()
{
    if (Instance == null)
        Instance = this;
    else
        Destroy(gameObject);
}



    private void Start()
    {
        foreach (Vector3Int pos in farmTilemap.cellBounds.allPositionsWithin)
        {
            if (farmTilemap.HasTile(pos))
                tileStates[pos] = FarmTileState.Default;
        }
    }

    // Hoe the tile
    public void HoeTile(Vector3 worldPos)
{
    Vector3Int cellPos = farmTilemap.WorldToCell(worldPos);

    // ✅ Restrict interaction to farm area
    if (!farmTilemap.cellBounds.Contains(cellPos))
    {
        Debug.Log("❌ Can't hoe outside the farm area!");
        return;
    }

    Debug.Log($"🪓 HoeTile CALLED at {cellPos}");
    
    if (!tileStates.ContainsKey(cellPos))
        tileStates[cellPos] = FarmTileState.Default;

    if (tileStates[cellPos] == FarmTileState.Default)
    {
        farmTilemap.SetTile(cellPos, plowedTile);
        farmTilemap.RefreshTile(cellPos);
        tileStates[cellPos] = FarmTileState.Plowed;
        Debug.Log($"✅ Plowed tile at {cellPos}");
    }
}

    // Plant seed
    public void PlantSeed(Vector3 worldPos, CropData cropData)
    {
        Vector3Int cellPos = farmTilemap.WorldToCell(worldPos);

        if (tileStates.ContainsKey(cellPos) && tileStates[cellPos] == FarmTileState.Plowed)
        {
            farmTilemap.SetTile(cellPos, seededTile);
            tileStates[cellPos] = FarmTileState.Seeded;

            Vector3 spawnPos = farmTilemap.GetCellCenterWorld(cellPos);
            GameObject cropObj = Instantiate(cropPrefab, spawnPos, Quaternion.identity);
            CropsBehaviour cropScript = cropObj.GetComponent<CropsBehaviour>();
            cropScript.Initialize(cropData, cellPos, this);
            plantedCrops[cellPos] = cropScript;
        }
    }

    // Water tile
    public void WaterTile(Vector3 worldPos)
    {
        Vector3Int cellPos = farmTilemap.WorldToCell(worldPos);

        if (plantedCrops.TryGetValue(cellPos, out CropsBehaviour crop))
        {
            if (playerAnimator != null)
                playerAnimator.SetTrigger("Water");

            crop.Water();

            farmTilemap.SetTile(cellPos, wateredSeedTile);
            tileStates[cellPos] = FarmTileState.Watered;
        }
    }

    // Harvest tile
    public void HarvestTile(Vector3 worldPos)
    {
        Vector3Int cellPos = farmTilemap.WorldToCell(worldPos);

        if (plantedCrops.TryGetValue(cellPos, out CropsBehaviour crop) && crop.IsGrown())
        {
            if (playerAnimator != null)
                playerAnimator.SetTrigger("Harvest");

            if (inventoryUI != null)
                inventoryUI.AddToolItem(crop.GetToolItem());

            Destroy(crop.gameObject);
            plantedCrops.Remove(cellPos);

            farmTilemap.SetTile(cellPos, plowedTile);
            tileStates[cellPos] = FarmTileState.Plowed;
        }
    }

    public FarmTileState GetTileState(Vector3Int pos)
    {
        if (tileStates.TryGetValue(pos, out FarmTileState state))
            return state;
        return FarmTileState.Default;
    }

    public void SetTileState(Vector3Int pos, FarmTileState newState)
    {
        tileStates[pos] = newState;

        switch (newState)
        {
            case FarmTileState.Default: farmTilemap.SetTile(pos, defaultTile); break;
            case FarmTileState.Plowed: farmTilemap.SetTile(pos, plowedTile); break;
            case FarmTileState.Seeded: farmTilemap.SetTile(pos, seededTile); break;
            case FarmTileState.Watered: farmTilemap.SetTile(pos, wateredSeedTile); break;
        }
    }

    public void CropDied(Vector3Int cellPos)
    {
        if (plantedCrops.ContainsKey(cellPos))
        {
            plantedCrops.Remove(cellPos);
            farmTilemap.SetTile(cellPos, plowedTile);
            tileStates[cellPos] = FarmTileState.Plowed;
        }
    }
}

public enum FarmTileState
{
    Default,
    Plowed,
    Seeded,
    Watered
}
